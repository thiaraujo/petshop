using System;
using System.Collections.Generic;

namespace Data.Entities.Models
{
    public partial class VendaServico
    {
        public int Id { get; set; }
        public int AgendamentoId { get; set; }
        public int? ServicoId { get; set; }
        public decimal Valor { get; set; }
        public decimal? ValorComDesconto { get; set; }

        public virtual Agendamento Agendamento { get; set; }
        public virtual Servico Servico { get; set; }
    }
}
