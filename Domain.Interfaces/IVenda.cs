using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Entities.Models;
using Data.Entities.ViewModels;

namespace Domain.Interfaces
{
    public interface IVenda : IBaseRepository<Venda>
    {
        Task RegistraVendaProduto(VendaProduto produto);
        Task<string> ConcretizaVenda(Venda venda);
        Task<IEnumerable<Venda>> ConsultaRegistros();
        Task<IEnumerable<VendaServicoViewModel>> ConsultaRegistrosPorCliente(int clienteId);
    }
}
