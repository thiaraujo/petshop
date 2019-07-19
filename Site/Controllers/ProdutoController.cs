using System.Linq;
using System.Threading.Tasks;
using Data.Entities.Models;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Middleware.Converters.Interface;
using Middleware.Converters.Service;
using Site.Abstraction;
using Site.Services;
using X.PagedList;

namespace Site.Controllers
{
    public class ProdutoController : AbstractController
    {
        #region Construtor

        const int TamanhoPagina = 15;
        private readonly IProduto _produto;
        private readonly IToastrMensagem _toastrMensagem;

        public ProdutoController(IProduto produto)
        {
            _produto = produto;
            _toastrMensagem = new ToastrMensagem();
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

            return View(new Produto());
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
                return RedirectToAction("Cadastro", new { produto.Id });
            }

            Toastr(_toastrMensagem.RegistroConfirmado());
            return RedirectToAction("Produtos");
        }

        #endregion
    }
}