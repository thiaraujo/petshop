using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities.Models;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Site.Abstraction;
using Site.Models;

namespace Site.Controllers
{
    public class RelatorioController : AbstractController
    {
        private readonly IVenda _vendas;
        private readonly IUsuario _usuario;

        public RelatorioController(IVenda vendas, IUsuario usuario)
        {
            _vendas = vendas;
            _usuario = usuario;
        }

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

        #region Auxiliar

        private async Task GetValores()
        {
            var profissionais = await _usuario.GetAllAsync();
            ViewBag.Profissionais = new SelectList(profissionais.OrderBy(x => x.Nome), "Id", "Nome");

            var meses = new MesesModel().GetMeses();
            ViewBag.Meses = new SelectList(meses, "Id", "Nome");
        }

        #endregion
    }
}