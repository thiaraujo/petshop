using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Site.Services
{
    public static class Upload
    {
        public static string SalvarArquivo(IFormFile file)
        {
            if (file == null) return null;

            var upload = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images/produtos/");
            if (!Directory.Exists(upload))
                Directory.CreateDirectory(upload);

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
                var fileName = parsedContentDisposition.FileName;

                var nome = DateTime.Now.Ticks + "-" + fileName.Value.Replace("\\", "").Replace("\"", "");
                var filePath = Path.Combine(upload, nome);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                return nome;
            }
        }
    }
}
