using System.Linq;
using System.Threading.Tasks;
using Data.Entities.Models;
using Data.Entities.ViewModels;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Middleware.Converters.Interface;
using Site.Abstraction;
using Site.Identity;
using X.PagedList;

namespace Site.Controllers
{
    [Authorize]
    public class ServicoController : AbstractController
    {
        #region Construtor

        const int TamanhoPagina = 15;
        private readonly IServico _servico;
        private readonly IProduto _produto;
        private readonly IServicoProduto _servicoProduto;
        private readonly IToastrMensagem _toastrMensagem;

        public ServicoController(IServico servico,
            IProduto produto,
            IServicoProduto servicoProduto,
            IToastrMensagem toastrMensagem)
        {
            _servico = servico;
            _produto = produto;
            _servicoProduto = servicoProduto;
            _toastrMensagem = toastrMensagem;
        }

        #endregion

        #region Consulta

        public async Task<IActionResult> Servicos(string s, int? p)
        {
            var listaDeRegistros = await _servico.ConsultaRegistros(null);
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
                var registroParaEdicao = await _servico.GetByIdAsync(id.Value);
                if (registroParaEdicao == null)
                {
                    Toastr(_toastrMensagem.ConsultaIncorreta());
                    return RedirectToAction("Servicos");
                }

                return View(registroParaEdicao);
            }

            return View(new Servico { Ativo = 1 });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastro(Servico servico, int[] produtos)
        {
            var cadastroEdicaoConfirmado = await _servico.CadastraOuAtualiza(servico, User.Identity.GetId());

            if (cadastroEdicaoConfirmado == null)
            {
                Toastr(_toastrMensagem.Aviso("Registro de cadastro inválido! Verifique os campos digitados e tente novamente."));
                return RedirectToAction("Cadastro", new { servico.Id });
            }

            if (produtos.Any())
                await _servicoProduto.CadastraOuAtualizaProdutosNoServico(servico.Id, produtos);

            Toastr(_toastrMensagem.RegistroConfirmado());
            return RedirectToAction("Servicos");
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
                var produtos = await _servicoProduto.ConsultaProdutosDoServico(id.Value);
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

        #endregion
    }
}