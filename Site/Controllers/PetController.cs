using System.Linq;
using System.Threading.Tasks;
using Data.Entities.Models;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Middleware.Converters.Interface;
using Site.Abstraction;
using X.PagedList;

namespace Site.Controllers
{
    [Authorize]
    public class PetController : AbstractController
    {
        #region Construtor

        const int TamanhoPagina = 15;
        private readonly IAnimal _animal;
        private readonly ICliente _cliente;
        private readonly ITipoAnimal _tipoAnimal;
        private readonly IRacaAnimal _racaAnimal;
        private readonly IPorteAnimal _porteAnimal;
        private readonly IToastrMensagem _toastrMensagem;

        public PetController(IAnimal animal,
            ICliente cliente,
            IToastrMensagem toastr,
            ITipoAnimal tipoAnimal,
            IRacaAnimal racaAnimal,
            IPorteAnimal porteAnimal)
        {
            _animal = animal;
            _cliente = cliente;
            _toastrMensagem = toastr;
            _tipoAnimal = tipoAnimal;
            _racaAnimal = racaAnimal;
            _porteAnimal = porteAnimal;
        }

        #endregion

        #region Consulta

        public async Task<IActionResult> Pets(int? p, string s)
        {
            var listaDeRegistros = await _animal.ConsultaRegistros(null);
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
                var registroParaEdicao = await _animal.GetByIdAsync(id.Value);
                if (registroParaEdicao == null)
                {
                    Toastr(_toastrMensagem.ConsultaIncorreta());
                    return RedirectToAction("Pets");
                }

                await CarregaDados(registroParaEdicao);
                return View(registroParaEdicao);
            }

            await CarregaDados(new Animal());
            return View(new Animal { AutorizaDivulgacao = 1 });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastro(Animal pet)
        {
            var cadastroEdicaoConfirmado = await _animal.CadastraOuAtualiza(pet);

            if (cadastroEdicaoConfirmado == null)
            {
                Toastr(_toastrMensagem.Aviso("Registro de cadastro inválido! Verifique os campos digitados e tente novamente."));
                if (pet.Id > 0)
                    return RedirectToAction("Cadastro", new { pet.Id });

                return View(pet);
            }

            Toastr(_toastrMensagem.RegistroConfirmado());
            return RedirectToAction("Pets");
        }

        #endregion

        #region Auxiliar

        private async Task CarregaDados(Animal pet)
        {
            var tipo = await _tipoAnimal.GetAllAsync(x => x.Ativo == 1);
            var raca = await _racaAnimal.GetAllAsync();
            var porte = await _porteAnimal.GetAllAsync();
            var cliente = await _cliente.GetAllAsync(x => x.Ativo == 1);

            ViewBag.Porte = new SelectList(porte.OrderBy(x => x.Nome), "Id", "Nome", pet.PorteAnimalId);
            ViewBag.Tipo = new SelectList(tipo.OrderBy(x => x.Nome), "Id", "Nome", pet.TipoAnimalId);
            ViewBag.Raca = new SelectList(raca.OrderBy(x => x.Nome), "Id", "Nome", pet.RacaAnimalId);

            var clientes = cliente.Select(x => new
            {
                x.Id,
                Nome = x.Nome + " - " + x.Cpf
            }).OrderBy(x => x.Nome).ToList();
            ViewBag.Cliente = new SelectList(clientes, "Id", "Nome", pet.ClienteId);
        }

        #endregion
    }
}