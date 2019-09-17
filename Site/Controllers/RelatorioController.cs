using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities.Models;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Site.Abstraction;
using Site.Models;

namespace Site.Controllers
{
    [Authorize]
    public class RelatorioController : AbstractController
    {
        #region Construtor

        private readonly IVenda _vendas;
        private readonly IUsuario _usuario;
        private readonly ICliente _cliente;

        public RelatorioController(IVenda vendas, IUsuario usuario, ICliente cliente)
        {
            _vendas = vendas;
            _usuario = usuario;
            _cliente = cliente;
        }

        #endregion

        #region Relatórios

        public async Task<IActionResult> Servicos(int? profissional, int? mes)
        {
            await GetValores();

            if (profissional.HasValue)
            {
                var listDeRegistros = await _vendas.ConsultaRegistros();
                listDeRegistros = listDeRegistros.Where(x => x.Agendamento.UsuarioId == profissional);
                listDeRegistros = listDeRegistros.Where(x => x.DataPagamento.Month == mes);
                return View(listDeRegistros.OrderBy(x => x.DataPagamento));
            }

            return View(new List<Venda>());
        }

        public async Task<IActionResult> ServicosCliente(int? clienteId, int? mes)
        {
            await GetValoresClientes();

            if (clienteId.HasValue)
            {
                var listDeRegistros = await _vendas.ConsultaRegistros();
                listDeRegistros = listDeRegistros.Where(x => x.Agendamento.ClienteId == clienteId);
                listDeRegistros = listDeRegistros.Where(x => x.DataPagamento.Month == mes);
                return View(listDeRegistros.OrderBy(x => x.DataPagamento));
            }

            return View(new List<Venda>());
        }

        #endregion

        #region Auxiliar

        private async Task GetValores()
        {
            var profissionais = await _usuario.GetAllAsync();
            ViewBag.Profissionais = new SelectList(profissionais.OrderBy(x => x.Nome), "Id", "Nome");

            var meses = new MesesModel().GetMeses();
            ViewBag.Meses = new SelectList(meses, "Id", "Nome");
        }

        private async Task GetValoresClientes()
        {
            var clientes = await _cliente.GetAllAsync();
            ViewBag.Clientes = new SelectList(clientes.OrderBy(x => x.Nome), "Id", "Nome");

            var meses = new MesesModel().GetMeses();
            ViewBag.Meses = new SelectList(meses, "Id", "Nome");
        }

        #endregion
    }
}