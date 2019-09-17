using System;
using System.Collections.Generic;

namespace Data.Entities.Models
{
    public partial class ClientePontuacao
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int? Pontos { get; set; }
        public DateTime? DataAtualizado { get; set; }

        public virtual Cliente Cliente { get; set; }
    }
}
