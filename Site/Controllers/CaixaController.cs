using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities.Models;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Middleware.Converters.Interface;
using Site.Abstraction;
using Site.Models;

namespace Site.Controllers
{
    [Authorize]
    public class CaixaController : AbstractController
    {
        #region Construtor

        private readonly ICliente _cliente;
        private readonly IAnimal _animal;
        private readonly IServico _servico;
        private readonly IUsuario _usuario;
        private readonly IPromocaoProdServ _promocao;
        private readonly ITipoPagamento _tipoPagamento;
        private readonly IVendaProduto _vendaProduto;
        private readonly IProduto _produtos;
        private readonly IVenda _venda;
        private readonly IAgendamento _agendamento;
        private readonly IClientePontuacao _clientePontuacao;
        private readonly IToastrMensagem _toastr;

        public CaixaController(ICliente cliente,
            IAnimal animal,
            IServico servico,
            IUsuario usuario,
            IAgendamento agendamento,
            ITipoPagamento tipoPagamento,
            IProduto produtos,
            IPromocaoProdServ promocao,
            IVendaProduto vendaProduto,
            IVenda venda, IToastrMensagem toastr, IClientePontuacao clientePontuacao)
        {
            _cliente = cliente;
            _animal = animal;
            _servico = servico;
            _usuario = usuario;
            _agendamento = agendamento;
            _tipoPagamento = tipoPagamento;
            _produtos = produtos;
            _promocao = promocao;
            _vendaProduto = vendaProduto;
            _venda = venda;
            _toastr = toastr;
            _clientePontuacao = clientePontuacao;
        }

        #endregion

        #region Caixa

        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarVenda(Venda venda)
        {
            if (venda.TipoPagamentoId < 1 || venda.ValorPago < 1)
            {
                Toastr(_toastr.Aviso("Você precisa informar o tipo do pagamento e o valor pago pelo cliente!"));
                return RedirectToAction("Registro");
            }

            var produtos = await _vendaProduto.GetAllAsync(x => x.AgendamentoId == venda.AgendamentoId);
            var agendamento = await _agendamento.GetByIdAsync(venda.AgendamentoId);

            decimal valor = 0;
            decimal? desconto = 0;

            foreach (var item in produtos)
            {
                valor += item.Valor * (item.Quantidade ?? 1);
                desconto += item.ValorComDesconto.HasValue ? (item.ValorComDesconto * (item.Quantidade ?? 1)) : 0;
            }

            venda.ValorProdutos = valor;
            venda.ValorProdutosDesconto = desconto;
            venda.ValorServico = agendamento.Servico.Preco;
            venda.DataPagamento = DateTime.Now;

            //Se a opção for pataz, é porque possui saldo, então não cobra o valor do serviço
            if (venda.TipoPagamentoId == 4)
                venda.ValorServico = 0;

            await _venda.ConcretizaVenda(venda);
            Toastr(_toastr.RegistroConfirmado());

            return RedirectToAction("Registro");
        }

        #endregion

        #region Json Aux

        [HttpGet]
        public async Task<JsonResult> GetClientes()
        {
            var clientes = await _cliente.GetAllAsync(x => x.Ativo == 1);
            return Json(clientes.Select(x => new
            {
                x.Id,
                nome = x.Nome + " - " + x.Cpf
            }).OrderBy(x => x.nome));
        }

        [HttpGet]
        public async Task<JsonResult> GetPets(int clienteId)
        {
            var pets = await _animal.GetAllAsync(x => x.ClienteId == clienteId);
            return Json(pets.Select(x => new
            {
                x.Id,
                nome = x.Nome
            }).OrderBy(x => x.nome));
        }

        [HttpGet]
        public async Task<JsonResult> GetServicos()
        {
            var servicos = await _servico.GetAllAsync(x => x.Ativo == 1);
            return Json(servicos.Select(x => new
            {
                x.Id,
                nome = x.Nome
            }).OrderBy(x => x.nome));
        }

        [HttpGet]
        public async Task<JsonResult> GetUsuarios()
        {
            var usuarios = await _usuario.GetAllAsync(x => x.Ativo == 1);
            return Json(usuarios.Select(x => new
            {
                x.Id,
                nome = x.Nome
            }).OrderBy(x => x.nome));
        }

        [HttpGet]
        public async Task<bool> GetUsuariosEspecialidade(int animalId, int usuarioId)
        {
            return await _agendamento.ConsultaProfissionalBaseadoNoAnimal(animalId, usuarioId);
        }

        [HttpGet]
        public async Task<TimeSpan> GetDisponivelAgendamento(int usuarioId, string data, string hora, int servicoId)
        {
            if (string.IsNullOrEmpty(data) || string.IsNullOrEmpty(hora))
                return TimeSpan.Zero;

            var dt = Convert.ToDateTime(data);
            var time = TimeSpan.Parse(hora);

            //pega os agendamentos baseados na data e no usuário
            var agendamentos = await _agendamento.HorarioAgendamentoDisponivel(usuarioId, dt, time, servicoId);
            return agendamentos;
        }

        [HttpGet]
        public async Task<JsonResult> GetAgendamento(int id)
        {
            var agendamento = await _agendamento.GetByIdAsync(id);

            var dinamica = new
            {
                id = agendamento.Id,
                animalId = agendamento.AnimalId,
                clienteId = agendamento.ClienteId,
                servicoId = agendamento.ServicoId,
                usuarioId = agendamento.UsuarioId,
                obs = agendamento.Observacao,
                dia = agendamento.DiaMarcado.ToShortDateString(),
                hora = agendamento.HoraMarcado
            };

            return Json(dinamica);
        }

        [HttpGet]
        public async Task<JsonResult> GetAgendamentos()
        {
            var agendamentos = await _agendamento.ConsultaRegistros(null, DateTime.Now.Date);

            //Remove os que foram concluidos
            var vendas = await _venda.GetAllAsync(x => x.DataPagamento == DateTime.Now.Date);
            agendamentos = agendamentos.Where(x => !vendas.Any(v => v.AgendamentoId == x.Id));

            var reultadoJson = agendamentos.Select(x => new
            {
                id = x.Id,
                pet = x.Animal.Nome,
                cliente = x.Cliente.Nome,
                hora = x.HoraMarcado.ToString("g"),
                horaFull = x.HoraMarcado,
                ausente = x.Ausente == 1
            }).OrderBy(x => x.horaFull).ToList();

            return Json(reultadoJson);
        }

        [HttpGet]
        public async Task<JsonResult> GetPagamentoDetalhes(int id)
        {
            var servico = await _agendamento.GetByIdAsync(id);
            var agendamentos = await _agendamento.GetAllAsync(x => x.ClienteId == servico.ClienteId);

            //Se tiver pataz, calcula quantos ele tem
            var pz = 0;
            if (agendamentos.Any())
            {
                var pataz = await _clientePontuacao.GetByIdAsync(x => x.ClienteId == servico.ClienteId);
                pz = pataz == null ? 0 : pataz.Pontos ?? 0;
            }

            var result = new
            {
                cliente = servico.Cliente.Nome,
                pet = servico.Animal.Nome,
                servico = servico.Servico.Nome,
                valor = servico.Servico.Preco.ToString("N"),
                pataz = pz,
                pzNecessario = servico.Servico.PatazNecessario ?? 0,
                pzFinal = servico.Servico.PatazRecebido ?? 0
            };
            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetTipoPagamento()
        {
            var tipo = await _tipoPagamento.GetAllAsync();
            return Json(tipo.OrderBy(x => x.Nome));
        }

        [HttpGet]
        public async Task<JsonResult> GetProdutos()
        {
            var produtos = await _produtos.GetAllAsync(x => x.Ativo == 1);
            var result = produtos.Select(x => new
            {
                id = x.Id,
                nome = x.Nome + " [" + x.Preco?.ToString("N") + "]"
            });

            return Json(result.OrderBy(x => x.id));
        }

        [HttpGet]
        public async Task<JsonResult> GetProdutosVenda(int id)
        {
            var produtos = await _vendaProduto.GetAllAsync(x => x.AgendamentoId == id);

            var result = produtos.Select(x => new
            {
                id = x.Id,
                produto = x.Produto.Nome,
                quantidade = x.Quantidade,
                valor = x.Valor.ToString("N"),
                desconto = x.ValorComDesconto.HasValue ? x.ValorComDesconto.Value.ToString("N") : "-",
                total = (x.ValorComDesconto.HasValue ? (x.ValorComDesconto * x.Quantidade) : (x.Valor * x.Quantidade))?.ToString("N")
            }).ToList();

            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetValorTotal(int id)
        {
            var produtos = await _vendaProduto.GetAllAsync(x => x.AgendamentoId == id);
            var servico = await _agendamento.GetByIdAsync(id);

            decimal valor = 0;
            decimal? desconto = 0;

            foreach (var item in produtos)
            {
                valor += item.Valor * (item.Quantidade ?? 1);
                desconto += item.ValorComDesconto.HasValue ? (item.ValorComDesconto * (item.Quantidade ?? 1)) : 0;
            }

            valor += servico.Servico.Preco;
            var total = valor - (desconto ?? 0);

            var result = new { total = total.ToString("N"), desconto = (desconto ?? 0).ToString("N") };
            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetConcluidos()
        {
            var vendas = await _venda.ConsultaRegistros();
            vendas = vendas.Where(x => x.DataPagamento == DateTime.Now.Date);

            var reultadoJson = vendas.Select(x => new
            {
                id = x.Id,
                pet = x.Agendamento.Animal.Nome,
                cliente = x.Agendamento.Cliente.Nome,
                valor = x.ValorPago.ToString("N")
            }).OrderBy(x => x.id).ToList();

            return Json(reultadoJson);
        }

        #endregion

        #region Json Post

        public async Task<bool> PostAddAgendamento(Agendamento agendamento)
        {
            var result = await _agendamento.CadastraOuAtualiza(agendamento);
            return result.Id > 0; //se for maior que zero, então cadastrou
        }

        [HttpPost]
        public async Task<bool> PostAddProduto(VendaProduto vendaProduto)
        {
            if (vendaProduto.AgendamentoId < 1 || !vendaProduto.ProdutoId.HasValue || vendaProduto.ProdutoId < 1)
                return false;

            //Pega o produto e já veirifica se possui desconto
            var produtoComValorAtualizado = await _produtos.RegistroDoProduto(vendaProduto.ProdutoId.Value);
            vendaProduto.ValorComDesconto = produtoComValorAtualizado.PrecoComDesconto;
            vendaProduto.Valor = produtoComValorAtualizado.Preco ?? 0 * (vendaProduto.Quantidade ?? 1);

            await _venda.RegistraVendaProduto(vendaProduto);
            return true;
        }

        [HttpPost]
        public async Task<bool> PostClienteAusente(int id)
        {
            await _agendamento.AgendamentoCancelado(id);
            return true;
        }

        #endregion

        #region Outros

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion
    }
}
