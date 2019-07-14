using System.Diagnostics;
using LazZiya.TagHelpers.Alerts;
using Microsoft.AspNetCore.Mvc;
using Site.Abstraction;
using Site.Models;

namespace Site.Controllers
{
    public class CaixaController : AbstractController
    {
        public IActionResult Registro()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
