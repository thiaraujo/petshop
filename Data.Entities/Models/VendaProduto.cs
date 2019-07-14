using System;
using System.Collections.Generic;

namespace Data.Entities.Models
{
    public partial class VendaProduto
    {
        public int Id { get; set; }
        public int VendaId { get; set; }
        public int? ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
        public decimal? ValorComDesconto { get; set; }

        public virtual Produto Produto { get; set; }
        public virtual Venda Venda { get; set; }
    }
}
