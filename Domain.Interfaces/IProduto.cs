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
        Task<ProdutoViewModel> RegistroDoProduto(int produtoId);
        Task<bool> AtualizaEstoque(int id, int estoque);
    }
}
