using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Entities.Models;

namespace Domain.Interfaces
{
    public interface IPromocao : IBaseRepository<Promocao>
    {
        Task<Promocao> CadastraOuAtualiza(Promocao promocao);
        Task<IEnumerable<Promocao>> ConsultaRegistros();
    }
}
