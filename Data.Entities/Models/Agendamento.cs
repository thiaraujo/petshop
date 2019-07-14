using System;
using System.Collections.Generic;

namespace Data.Entities.Models
{
    public partial class Agendamento
    {
        public Agendamento()
        {
            Venda = new HashSet<Venda>();
        }

        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int? AnimalId { get; set; }
        public int? ServicoId { get; set; }
        public int? UsuarioId { get; set; }
        public DateTime DiaMarcado { get; set; }
        public TimeSpan HoraMarcado { get; set; }
        public string Observacao { get; set; }
        public int? Ausente { get; set; }

        public virtual Animal Animal { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual Servico Servico { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<Venda> Venda { get; set; }
    }
}
