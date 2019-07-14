using System;
using System.Collections.Generic;

namespace Data.Entities.Models
{
    public partial class ServicoLog
    {
        public int Id { get; set; }
        public int ServicoId { get; set; }
        public int UsuarioId { get; set; }
        public decimal PrecoAntigo { get; set; }
        public decimal? PrecoNovo { get; set; }
        public DateTime DataAlteracao { get; set; }

        public virtual Servico Servico { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
