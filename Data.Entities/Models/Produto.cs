using System;
using System.Collections.Generic;

namespace Data.Entities.Models
{
    public partial class Produto
    {
        public Produto()
        {
            Promocao = new HashSet<Promocao>();
            ServicoProduto = new HashSet<ServicoProduto>();
            VendaProduto = new HashSet<VendaProduto>();
        }

        public int Id { get; set; }
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Fabricante { get; set; }
        public string Especificacao { get; set; }
        public decimal? Preco { get; set; }
        public int? Estoque { get; set; }
        public DateTime DataCadastro { get; set; }
        public string Foto { get; set; }
        public int Ativo { get; set; }

        public virtual ICollection<Promocao> Promocao { get; set; }
        public virtual ICollection<ServicoProduto> ServicoProduto { get; set; }
        public virtual ICollection<VendaProduto> VendaProduto { get; set; }
    }
}
