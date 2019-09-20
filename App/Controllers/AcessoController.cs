using System;
using System.Security.Claims;
using System.Threading.Tasks;
using App.Abstraction;
using App.Identity;
using Data.Entities.Models;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Middleware.Converters.Interface;

namespace App.Controllers
{
    public class AcessoController : AbstractController
    {
        #region Construtor

        private readonly IToastrMensagem _toastrMensagem;
        private readonly ICliente _cliente;

        public AcessoController(ICliente cliente, IToastrMensagem toastrMensagem)
        {
            _cliente = cliente;
            _toastrMensagem = toastrMensagem;
        }

        #endregion

        public IActionResult Acessar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Acessar(string usuario, string email)
        {
            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(email))
            {
                Toastr(_toastrMensagem.CamposEmBranco());
                return View();
            }

            try
            {
                var registroUsuario = await _cliente.GetTopOneAsync(x => x.Cpf == usuario && x.Email == email);
                if (registroUsuario == null)
                {
                    Toastr(_toastrMensagem.Aviso("CPF ou E-mail invalidos!"));
                    return View();
                }

                await AddClaimsLogin(registroUsuario);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                Toastr(_toastrMensagem.Aviso("CPF ou E-mail invalidos!"));
                return View();
            }
        }

        public async Task<IActionResult> Sair()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return View();
        }

        #region Claims

        private async Task<bool> AddClaimsLogin(Cliente cliente)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

            identity.AddClaim(new Claim(IdentityExtensions.CustomClaimTypes.Id, cliente.Id.ToString()));
            identity.AddClaim(new Claim(IdentityExtensions.CustomClaimTypes.Nome, cliente.Nome));
            identity.AddClaim(new Claim(IdentityExtensions.CustomClaimTypes.Nome, cliente.Cpf));

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