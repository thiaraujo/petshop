using System;
using System.Collections.Generic;

namespace Data.Entities.Models
{
    public partial class Animal
    {
        public Animal()
        {
            Agendamento = new HashSet<Agendamento>();
        }

        public int Id { get; set; }
        public int? TipoAnimalId { get; set; }
        public int? RacaAnimalId { get; set; }
        public int? PorteAnimalId { get; set; }
        public int ClienteId { get; set; }
        public string Nome { get; set; }
        public string Alergia { get; set; }
        public string Detalhes { get; set; }
        public int? AutorizaDivulgacao { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual PorteAnimal PorteAnimal { get; set; }
        public virtual RacaAnimal RacaAnimal { get; set; }
        public virtual TipoAnimal TipoAnimal { get; set; }
        public virtual ICollection<Agendamento> Agendamento { get; set; }
    }
}
