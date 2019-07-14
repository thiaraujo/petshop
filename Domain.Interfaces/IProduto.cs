using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Entities.Models;
using Data.Entities.ViewModels;

namespace Domain.Interfaces
{
    public interface IProduto : IBaseRepository<Produto>
    {
        Task<Produto> CadastraOuAtualiza(Produto produto);
        Task<IEnumerable<Produto>> ConsultaRegistros(string produto);
        Task MovimentacaoEstoque(int produtoId, int? entrada, int? saida);
        Task DesabilitarRegistro(int produtoId);
        Task<ProdutoViewModel> RegistroDoProduto(int produtoId);
    }
}
