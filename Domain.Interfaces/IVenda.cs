using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Entities.Models;

namespace Domain.Interfaces
{
    public interface IVenda : IBaseRepository<Venda>
    {
        Task RegistraVendaProduto(VendaProduto produto);
        Task<string> ConcretizaVenda(Venda venda);
        Task<IEnumerable<Venda>> ConsultaRegistros();
    }
}
