using System;
using System.Collections.Generic;

namespace Data.Entities.Models
{
    public partial class TipoAnimal
    {
        public TipoAnimal()
        {
            Animal = new HashSet<Animal>();
            UsuarioEspecialidade = new HashSet<UsuarioEspecialidade>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public int Ativo { get; set; }

        public virtual ICollection<Animal> Animal { get; set; }
        public virtual ICollection<UsuarioEspecialidade> UsuarioEspecialidade { get; set; }
    }
}
