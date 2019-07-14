using System;
using System.Collections.Generic;

namespace Data.Entities.Models
{
    public partial class VendaServico
    {
        public int Id { get; set; }
        public int VendaId { get; set; }
        public int? ServicoId { get; set; }
        public decimal Valor { get; set; }
        public decimal? ValorComDesconto { get; set; }

        public virtual Servico Servico { get; set; }
        public virtual Venda Venda { get; set; }
    }
}
