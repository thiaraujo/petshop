using System;
using System.Collections.Generic;

namespace Data.Entities.Models
{
    public partial class TipoPagamento
    {
        public TipoPagamento()
        {
            Venda = new HashSet<Venda>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }

        public virtual ICollection<Venda> Venda { get; set; }
    }
}
