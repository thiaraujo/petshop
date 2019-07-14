using System;
using System.Collections.Generic;

namespace Data.Entities.Models
{
    public partial class ServicoUsuario
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int ServicoId { get; set; }

        public virtual Servico Servico { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
