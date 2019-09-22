using System.Linq;
using System.Threading.Tasks;
using Data.Entities.Models;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Middleware.Converters.Interface;
using Site.Abstraction;
using X.PagedList;

namespace Site.Controllers
{
    [Authorize]
    public class ClienteController : AbstractController
    {
        #region Construtor

        const int TamanhoPagina = 15;
        private readonly ICliente _cliente;
        private readonly IToastrMensagem _toastrMensagem;

        public ClienteController(ICliente cliente,
            IToastrMensagem toastrMensagem)
        {
            _cliente = cliente;
            _toastrMensagem = toastrMensagem;
        }

        #endregion

        #region Consulta

        public async Task<IActionResult> Clientes(string s, int? p)
        {
            var listaDeRegistros = await _cliente.ConsultaRegistros(null);
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
                var registroParaEdicao = await _cliente.GetByIdAsync(id.Value);
                if (registroParaEdicao == null)
                {
                    Toastr(_toastrMensagem.ConsultaIncorreta());
                    return RedirectToAction("Clientes");
                }

                return View(registroParaEdicao);
            }

            return View(new Cliente());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastro(Cliente cliente)
        {
            var cadastroEdicaoConfirmado = await _cliente.CadastraOuAtualiza(cliente);

            if (cadastroEdicaoConfirmado == null)
            {
                Toastr(_toastrMensagem.Aviso("Registro de cadastro inválido! Verifique os campos digitados e tente novamente."));
                return RedirectToAction("Cadastro", new { cliente.Id });
            }

            Toastr(_toastrMensagem.RegistroConfirmado());
            return RedirectToAction("Clientes");
        }

        #endregion
    }
}