using System;
using System.Collections.Generic;

namespace Data.Entities.Models
{
    public partial class Promocao
    {
        public Promocao()
        {
            PromocaoProdServ = new HashSet<PromocaoProdServ>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal? Percentual { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }

        public virtual ICollection<PromocaoProdServ> PromocaoProdServ { get; set; }
    }
}
