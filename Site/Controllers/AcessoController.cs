using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Data.Entities.Models;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Middleware.Converters;
using Middleware.Converters.Interface;
using Site.Abstraction;
using Site.Identity;

namespace Site.Controllers
{
    public class AcessoController : AbstractController
    {
        #region Construtor

        private readonly IUsuario _usuario;
        private readonly IToastrMensagem _toastrMensagem;

        public AcessoController(IUsuario usuario,
            IToastrMensagem toastrMensagem)
        {
            _usuario = usuario;
            _toastrMensagem = toastrMensagem;
        }

        #endregion

        public IActionResult Acessar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Acessar(string usuario, string senha)
        {
            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(senha))
            {
                Toastr(_toastrMensagem.CamposEmBranco());
                return View();
            }

            var passw = senha.EncriptText();
            try
            {
                var codigo = Convert.ToInt32(usuario);
                var registroUsuario = await _usuario.GetTopOneAsync(x => x.CodigoAcesso == codigo && x.SenhaAcesso == passw);
                if (registroUsuario == null)
                {
                    Toastr(_toastrMensagem.Aviso("Usuário ou senha inválidos!"));
                    return View();
                }

                await AddClaimsLogin(registroUsuario);
                return RedirectToAction("Registro", "Caixa");
            }
            catch (Exception)
            {
                Toastr(_toastrMensagem.Aviso("Usuário ou senha inválidos!"));
                return View();
            }
        }

        public async Task<IActionResult> Sair()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return View();
        }

        #region Claims

        private async Task<bool> AddClaimsLogin(Usuario operador)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

            identity.AddClaim(new Claim(IdentityExtensions.CustomClaimTypes.Id, operador.Id.ToString()));
            identity.AddClaim(new Claim(IdentityExtensions.CustomClaimTypes.Nome, operador.Nome));
            identity.AddClaim(new Claim(IdentityExtensions.CustomClaimTypes.Veterinario, (operador.EhVet == 1 ? "False" : "True")));
            identity.AddClaim(new Claim(IdentityExtensions.CustomClaimTypes.Administrador, (operador.EhAdministrador == 1 ? "False" : "True")));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties
                {
                    IsPersistent = true
                });

            return true;
        }

        #endregion
    }
}