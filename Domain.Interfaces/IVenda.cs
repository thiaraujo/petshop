using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Entities.Models;
using Data.Entities.ViewModels;

namespace Domain.Interfaces
{
    public interface IVenda : IBaseRepository<Venda>
    {
        Task<Venda> GerarVendaAposAgendamento(Agendamento agendamento);
        Task RegistraOpcaoPagamento(int pagamentoId, int vendaId);
        Task RegistraVendaProduto(VendaProduto produto);
        Task RegistraVendaServico(IEnumerable<ServicoViewModel> servicos, int agendamentoId);
        Task<string> ConcretizaVenda(Venda venda);
        Task<IEnumerable<Venda>> ConsultaRegistros();
    }
}
