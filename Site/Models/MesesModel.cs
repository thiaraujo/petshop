using System.Collections.Generic;

namespace Site.Models
{
    public class MesesModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public IEnumerable<MesesModel> GetMeses()
        {
            var meses = new List<MesesModel>
            {
                new MesesModel{ Id = 1, Nome = "Janeiro"},
                new MesesModel{ Id = 2, Nome = "Fevereiro"},
                new MesesModel{ Id = 3, Nome = "Março"},
                new MesesModel{ Id = 4, Nome = "Abril"},
                new MesesModel{ Id = 5, Nome = "Maio"},
                new MesesModel{ Id = 6, Nome = "Junho"},
                new MesesModel{ Id = 7, Nome = "Julho"},
                new MesesModel{ Id = 8, Nome = "Agosto"},
                new MesesModel{ Id = 9, Nome = "Setembro"},
                new MesesModel{ Id = 10, Nome = "Outubro"},
                new MesesModel{ Id = 11, Nome = "Novembro"},
                new MesesModel{ Id = 12, Nome = "Dezembro"},
            };

            return meses;
        }
    }
}
