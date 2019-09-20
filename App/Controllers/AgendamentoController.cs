using System;
using System.Threading.Tasks;
using App.Abstraction;
using App.Identity;
using Data.Entities.Models;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [Authorize]
    public class AgendamentoController : AbstractController
    {
        #region Construtor

        private readonly IVenda _venda;
        private readonly IClientePontuacao _clientePontuacao;
        private readonly IVendaAvaliacao _vendaAvaliacao;

        public AgendamentoController(IVenda venda,
            IClientePontuacao clientePontuacao,
            IVendaAvaliacao vendaAvaliacao)
        {
            _venda = venda;
            _clientePontuacao = clientePontuacao;
            _vendaAvaliacao = vendaAvaliacao;
        }

        #endregion

        public async Task<IActionResult> Agendamentos()
        {
            //Consulta os serviços realizados
            var servicosDoCliente = await _venda.ConsultaRegistrosPorCliente(User.Identity.GetId());

            //Consulta a quantidade de pontos petz
            await ConsultaPetz();

            return View(servicosDoCliente);
        }

        #region Auxiliar

        // Função que salva em uma bag temporária a pontuação do cliente
        private async Task ConsultaPetz()
        {
            var pontos = await _clientePontuacao.GetByIdAsync(x => x.ClienteId == User.Identity.GetId());

            ViewBag.Pontos = pontos.Pontos ?? 0;
        }

        #endregion

        #region Post

        //Função para registrar a avaliação que o cliente deu para o serviço
        [HttpPost]
        public async Task<bool> PostAvaliacao(VendaAvaliacao vendaAvaliacao)
        {
            if (vendaAvaliacao.AgendamentoId > 0 && vendaAvaliacao.Nota.HasValue && (vendaAvaliacao.Nota.Value >= 0 && vendaAvaliacao.Nota.Value <= 10))
            {
                vendaAvaliacao.DataAvaliado = DateTime.Now;
                await _vendaAvaliacao.AddAsync(vendaAvaliacao);
                return true;
            }

            return false;
        }

        #endregion
    }
}