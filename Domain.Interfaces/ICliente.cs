using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Entities.Models;

namespace Domain.Interfaces
{
    public interface ICliente : IBaseRepository<Cliente>
    {
        Task<Cliente> CadastraOuAtualiza(Cliente cliente);
        Task<IEnumerable<Cliente>> ConsultaRegistros(string cliente);
        Task DesabilitarRegistro(int clienteId);
    }
}
