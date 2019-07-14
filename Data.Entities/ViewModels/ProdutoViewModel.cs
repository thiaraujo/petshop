
namespace Data.Entities.ViewModels
{
    public class ProdutoViewModel
    {
        public int ProdutoId { get; set; }
        public decimal? Preco { get; set; }
        public decimal? PrecoComDesconto { get; set; }
        public double PercentualDesconto { get; set; }
    }
}
