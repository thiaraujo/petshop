﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Entities.Models;

namespace Domain.Interfaces
{
    public interface IUsuario : IBaseRepository<Usuario>
    {
        Task<Usuario> CadastraOuAtualiza(Usuario usuario, int[] tipoPet);
        Task<IEnumerable<Usuario>> ConsultaRegistros(string usuario);
    }
}
