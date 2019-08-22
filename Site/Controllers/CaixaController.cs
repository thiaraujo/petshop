using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities.Models;
using Domain.Interfaces;
using LazZiya.TagHelpers.Alerts;
using Microsoft.AspNetCore.Mvc;
using Site.Abstraction;
using Site.Models;

namespace Site.Controllers
{
    public class CaixaController : AbstractController
    {
        #region Construtor

        private readonly ICliente _cliente;
        private readonly IAnimal _animal;
        private readonly IServico _servico;
        private readonly IUsuario _usuario;
        private readonly IUsuarioEspecialidade _especialidade;
        private readonly IAgendamento _agendamento;

        public CaixaController(ICliente cliente,
            IAnimal animal,
            IServico servico,
            IUsuario usuario, IUsuarioEspecialidade especialidade, IAgendamento agendamento)
        {
            _cliente = cliente;
            _animal = animal;
            _servico = servico;
            _usuario = usuario;
            _especialidade = especialidade;
            _agendamento = agendamento;
        }

        #endregion

        public IActionResult Registro()
        {
            return View();
        }

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
            var dt = Convert.ToDateTime(data);
            var time = TimeSpan.Parse(hora);

            //pega os agendamentos baseados na data e no usuário
            var agendamentos = await _agendamento.HorarioAgendamentoDisponivel(usuarioId, dt, time, servicoId);
            return agendamentos;
        }

        #endregion

        #region Json Post

        public async Task<bool> PostAddAgendamento(Agendamento agendamento)
        {
            var result = await _agendamento.CadastraOuAtualiza(agendamento);
            return result.Id > 0; //se for maior que zero, então cadastrou
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
