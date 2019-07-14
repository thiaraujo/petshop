using System;
using System.Collections.Generic;

namespace Data.Entities.Models
{
    public partial class UsuarioEspecialidade
    {
        public int Id { get; set; }
        public int TipoAnimalId { get; set; }
        public int UsuarioId { get; set; }

        public virtual TipoAnimal TipoAnimal { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
