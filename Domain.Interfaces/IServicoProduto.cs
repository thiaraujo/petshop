using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Entities.Models;

namespace Domain.Interfaces
{
    public interface IServicoProduto : IBaseRepository<ServicoProduto>
    {
        Task CadastraOuAtualizaProdutosNoServico(int servicoId, int[] produtos);
        Task<IEnumerable<ServicoProduto>> ConsultaProdutosDoServico(int servicoId);
    }
}
