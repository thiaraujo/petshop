using Microsoft.AspNetCore.Mvc;
using Middleware.Converters.Models;

namespace Site.Abstraction
{
    public class AbstractController : Controller
    {
        public void Toastr(PayloadMensagem mensagem)
        {
            TempData["toastr"] = mensagem.Tipo;
            TempData["toastrMsg"] = mensagem.Mensagem;
            TempData["toastrTitulo"] = mensagem.Titulo;
        }
    }
}
