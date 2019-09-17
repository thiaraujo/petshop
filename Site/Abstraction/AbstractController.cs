﻿using Microsoft.AspNetCore.Mvc;
using Middleware.Converters.Models;

namespace Site.Abstraction
{
    public class AbstractController : Controller
    {
        public void Toastr(Payload payloadMensagem)
        {
            TempData["toastr"] = payloadMensagem.Tipo;
            TempData["toastrMsg"] = payloadMensagem.Mensagem;
            TempData["toastrTitulo"] = payloadMensagem.Titulo;
        }
    }
}
