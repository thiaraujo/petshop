using System;
using System.Collections.Generic;

namespace Data.Entities.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            Agendamento = new HashSet<Agendamento>();
            Animal = new HashSet<Animal>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Rg { get; set; }
        public string Cpf { get; set; }
        public string Endereco { get; set; }
        public string Email { get; set; }
        public DateTime DataCadastro { get; set; }
        public int Ativo { get; set; }

        public virtual ICollection<Agendamento> Agendamento { get; set; }
        public virtual ICollection<Animal> Animal { get; set; }
    }
}
