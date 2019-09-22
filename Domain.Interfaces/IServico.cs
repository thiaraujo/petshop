using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Entities.Models;

namespace Domain.Interfaces
{
    public interface IServico : IBaseRepository<Servico>
    {
        Task<Servico> CadastraOuAtualiza(Servico servico, int usuarioAlterando);
        Task<IEnumerable<Servico>> ConsultaRegistros(string servico);
    }
}
