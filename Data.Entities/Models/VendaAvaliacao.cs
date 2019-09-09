using System;
using System.Collections.Generic;

namespace Data.Entities.Models
{
    public partial class VendaAvaliacao
    {
        public int Id { get; set; }
        public int AgendamentoId { get; set; }
        public int? Nota { get; set; }

        public virtual Agendamento Agendamento { get; set; }
    }
}
