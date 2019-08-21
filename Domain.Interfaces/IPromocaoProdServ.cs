using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Entities.Models;

namespace Domain.Interfaces
{
    public interface IPromocaoProdServ : IBaseRepository<PromocaoProdServ>
    {
        Task<IEnumerable<PromocaoProdServ>> ConsultaRegistros(int promocaoId);
        Task CadastraOuAtualizaProdServNaPromocao(int promocaoId, int[] produtos, int[] servicos);
    }
}
