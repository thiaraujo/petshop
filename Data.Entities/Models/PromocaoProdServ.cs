using System;
using System.Collections.Generic;

namespace Data.Entities.Models
{
    public partial class PromocaoProdServ
    {
        public int Id { get; set; }
        public int PromocaoId { get; set; }
        public int? ProdutoId { get; set; }
        public int? ServicoId { get; set; }

        public virtual Produto Produto { get; set; }
        public virtual Promocao Promocao { get; set; }
        public virtual Servico Servico { get; set; }
    }
}
