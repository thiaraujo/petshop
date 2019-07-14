using System;
using System.Collections.Generic;

namespace Data.Entities.Models
{
    public partial class Promocao
    {
        public int Id { get; set; }
        public int? ProdutoId { get; set; }
        public int? ServicoId { get; set; }
        public decimal? Percentual { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }

        public virtual Produto Produto { get; set; }
        public virtual Servico Servico { get; set; }
    }
}
