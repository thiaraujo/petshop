using System;
using System.Collections.Generic;

namespace Data.Entities.Models
{
    public partial class VendaAvaliacao
    {
        public int Id { get; set; }
        public int VendaId { get; set; }
        public int? Nota { get; set; }

        public virtual Venda Venda { get; set; }
    }
}
