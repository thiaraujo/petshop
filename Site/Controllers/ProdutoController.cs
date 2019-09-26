using System.Linq;
using System.Threading.Tasks;
using Data.Entities.Models;
using Data.Entities.ViewModels;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Middleware.Converters.Interface;
using Site.Abstraction;
using Site.Services;
using X.PagedList;

namespace Site.Controllers
{
    [Authorize]
    public class ProdutoController : AbstractController
    {
        #region Construtor

        const int TamanhoPagina = 15;
        private readonly IProduto _produto;
        private readonly IServico _servico;
        private readonly IPromocao _promocao;
        private readonly IPromocaoProdServ _promocaoProdServ;
        private readonly IToastrMensagem _toastrMensagem;

        public ProdutoController(IProduto produto,
            IToastrMensagem toastrMensagem,
            IPromocao promocao,
            IPromocaoProdServ promocaoProdServ,
            IServico servico)
        {
            _produto = produto;
            _toastrMensagem = toastrMensagem;
            _promocao = promocao;
            _promocaoProdServ = promocaoProdServ;
            _servico = servico;
        }

        #endregion

        #region Consulta

        public async Task<IActionResult> Produtos(string s, int? p)
        {
            var listaDeRegistros = await _produto.ConsultaRegistros(null);
            var numPagina = p ?? 1;

            if (string.IsNullOrEmpty(s))
            {
                return View(listaDeRegistros.ToPagedList(numPagina, TamanhoPagina));
            }

            listaDeRegistros = listaDeRegistros.Where(x => x.Nome.ToLower().Contains(s.ToLower()));

            return View(listaDeRegistros.ToPagedList(numPagina, TamanhoPagina));
        }

        #endregion

        #region Promoções Consulta

        public async Task<IActionResult> Promocoes(string s, int? p)
        {
            var listaDeRegistros = await _promocao.ConsultaRegistros();
            var numPagina = p ?? 1;

            if (string.IsNullOrEmpty(s))
            {
                return View(listaDeRegistros.ToPagedList(numPagina, TamanhoPagina));
            }

            listaDeRegistros = listaDeRegistros.Where(x => x.Nome.ToLower().Contains(s.ToLower()));

            return View(listaDeRegistros.ToPagedList(numPagina, TamanhoPagina));
        }

        #endregion

        #region Cadastro

        public async Task<IActionResult> Cadastro(int? id)
        {
            if (id.HasValue)
            {
                var registroParaEdicao = await _produto.GetByIdAsync(id.Value);
                if (registroParaEdicao == null)
                {
                    Toastr(_toastrMensagem.ConsultaIncorreta());
                    return RedirectToAction("Produtos");
                }

                return View(registroParaEdicao);
            }

            return View(new Produto { Ativo = 1 });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastro(Produto produto, IFormFile file)
        {
            var imagem = string.Empty;
            if (file != null)
            {
                imagem = Upload.SalvarArquivo(file);
                produto.Foto = imagem;
            }

            var cadastroEdicaoConfirmado = await _produto.CadastraOuAtualiza(produto);

            if (cadastroEdicaoConfirmado == null)
            {
                Toastr(_toastrMensagem.Aviso("Registro de cadastro inválido! Verifique os campos digitados e tente novamente."));
                if (produto.Id > 0)
                    return RedirectToAction("Cadastro", new { produto.Id });

                return View(produto);
            }

            Toastr(_toastrMensagem.RegistroConfirmado());
            return RedirectToAction("Produtos");
        }

        #endregion

        #region Promoções Cadastro

        public async Task<IActionResult> CadastroPromocao(int? id)
        {
            if (id.HasValue)
            {
                var registroParaEdicao = await _promocao.GetByIdAsync(id.Value);
                if (registroParaEdicao == null)
                {
                    Toastr(_toastrMensagem.ConsultaIncorreta());
                    return RedirectToAction("Promocoes");
                }

                return View(registroParaEdicao);
            }

            return View(new Promocao());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CadastroPromocao(Promocao promocao)
        {
            var cadastroEdicaoConfirmado = await _promocao.CadastraOuAtualiza(promocao);

            if (cadastroEdicaoConfirmado == null)
            {
                Toastr(_toastrMensagem.Aviso("Registro de cadastro inválido! Verifique os campos digitados e tente novamente."));
                return RedirectToAction("CadastroPromocao", new { promocao.Id });
            }

            Toastr(_toastrMensagem.RegistroConfirmado());
            return RedirectToAction("Promocoes", new { id = promocao.Id });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CadastroPromocaoProdServ(int[] produtos, int[] servicos, int promocaoId)
        {
            if (promocaoId < 1)
            {
                Toastr(_toastrMensagem.Aviso("Registro de cadastro inválido! Verifique os campos digitados e tente novamente."));
                return RedirectToAction("Promocoes");
            }

            //Atualiza os registros
            await _promocaoProdServ.CadastraOuAtualizaProdServNaPromocao(promocaoId, produtos, servicos);

            Toastr(_toastrMensagem.RegistroConfirmado());
            return RedirectToAction("Promocoes", new { id = promocaoId });
        }

        #endregion

        #region Json

        [HttpGet]
        public async Task<JsonResult> GetProdutos(int? id)
        {
            var listaProdutos = await _produto.GetAllAsync(x => x.Ativo == 1);

            //Converte o objeto para uma viewmodel
            var listaRetornoProdutos = listaProdutos.Select(item => new ProdutoServicoViewModel
            {
                Id = item.Id,
                Nome = item.Nome,
                Possui = false
            }).ToList();

            //Se tiver o usuárioId preenchido, quer dizer que é uma edição
            if (id.HasValue)
            {
                var produtos = await _promocaoProdServ.ConsultaRegistros(id.Value);
                if (produtos.Any())
                {
                    foreach (var item in listaRetornoProdutos)
                    {
                        if (produtos.Any(x => x.ProdutoId == item.Id))
                            item.Possui = true;
                    }
                }
            }

            return Json(listaRetornoProdutos);
        }

        [HttpGet]
        public async Task<JsonResult> GetServicos(int? id)
        {
            var listaProdutos = await _servico.GetAllAsync(x => x.Ativo == 1);

            //Converte o objeto para uma viewmodel
            var listaRetornoProdutos = listaProdutos.Select(item => new ProdutoServicoViewModel
            {
                Id = item.Id,
                Nome = item.Nome,
                Possui = false
            }).ToList();

            //Se tiver o usuárioId preenchido, quer dizer que é uma edição
            if (id.HasValue)
            {
                var servicos = await _promocaoProdServ.ConsultaRegistros(id.Value);
                if (servicos.Any())
                {
                    foreach (var item in listaRetornoProdutos)
                    {
                        if (servicos.Any(x => x.ServicoId == item.Id))
                            item.Possui = true;
                    }
                }
            }

            return Json(listaRetornoProdutos);
        }

        #endregion
    }
}