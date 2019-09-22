using System.Linq;
using System.Threading.Tasks;
using Data.Entities.Models;
using Data.Entities.ViewModels;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Middleware.Converters.Interface;
using Site.Abstraction;
using X.PagedList;

namespace Site.Controllers
{
    [Authorize]
    public class ProfissionalController : AbstractController
    {
        #region Construtor

        const int TamanhoPagina = 15;
        private readonly IUsuario _usuario;
        private readonly ITipoAnimal _tipoAnimal;
        private readonly IUsuarioEspecialidade _usuarioEspecialidade;
        private readonly IToastrMensagem _toastrMensagem;

        public ProfissionalController(IUsuario usuario,
            ITipoAnimal tipoAnimal,
            IUsuarioEspecialidade usuarioEspecialidade,
            IToastrMensagem toastrMensagem)
        {
            _usuario = usuario;
            _tipoAnimal = tipoAnimal;
            _usuarioEspecialidade = usuarioEspecialidade;
            _toastrMensagem = toastrMensagem;
        }

        #endregion

        #region Consulta

        public async Task<IActionResult> Profissionais(string s, int? p)
        {
            var listaDeRegistros = await _usuario.ConsultaRegistros(null);
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
                var registroParaEdicao = await _usuario.GetByIdAsync(id.Value);
                if (registroParaEdicao == null)
                {
                    Toastr(_toastrMensagem.ConsultaIncorreta());
                    return RedirectToAction("Profissionais");
                }

                return View(registroParaEdicao);
            }

            return View(new Usuario { Ativo = 1 });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastro(Usuario usuario, int[] especialidades)
        {
            var cadastroEdicaoConfirmado = await _usuario.CadastraOuAtualiza(usuario, especialidades);

            if (cadastroEdicaoConfirmado == null)
            {
                Toastr(_toastrMensagem.Aviso("Registro de cadastro inválido! Verifique os campos digitados e tente novamente."));
                return RedirectToAction("Cadastro", new { usuario.Id });
            }

            // Converte para array para não perder a referência
            if (especialidades.Any())
                await _usuarioEspecialidade.CadastraOuAtualizaEspecialidade(usuario.Id, especialidades);

            Toastr(_toastrMensagem.RegistroConfirmado());
            return RedirectToAction("Profissionais");
        }

        #endregion

        #region Json 

        [HttpGet]
        public async Task<JsonResult> CarregaTipoDeAnimais(int? id)
        {
            var listaTipoAnimais = await _tipoAnimal.GetAllAsync(x => x.Ativo == 1);

            //Converte o objeto para uma viewmodel
            var listaRetornoAnimais = listaTipoAnimais.Select(item => new TipoAnimalViewModel
            {
                Id = item.Id,
                Nome = item.Nome,
                Possui = false
            }).ToList();

            //Se tiver o usuárioId preenchido, quer dizer que é uma edição
            if (id.HasValue)
            {
                var especialidades = await _usuarioEspecialidade.ConsultaEspecialidadesUsuario(id.Value);
                if (especialidades.Any())
                {
                    foreach (var item in listaRetornoAnimais)
                    {
                        if (especialidades.Any(x => x.TipoAnimalId == item.Id))
                            item.Possui = true;
                    }
                }
            }

            return Json(listaRetornoAnimais);
        }

        #endregion
    }
}