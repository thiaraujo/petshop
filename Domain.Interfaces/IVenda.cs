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
        Task RegistraVendaProduto(IEnumerable<ProdutoViewModel> produtos, int agendamentoId);
        Task RegistraVendaServico(IEnumerable<ServicoViewModel> servicos, int agendamentoId);
        Task<string> ConcretizaVenda(decimal valorPago, int? pontosUtilizados, int vendaId);
        Task<IEnumerable<Venda>> ConsultaRegistros();
    }
}
