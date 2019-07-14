using System;
using System.Collections.Generic;

namespace Data.Entities.Models
{
    public partial class PorteAnimal
    {
        public PorteAnimal()
        {
            Animal = new HashSet<Animal>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }

        public virtual ICollection<Animal> Animal { get; set; }
    }
}
