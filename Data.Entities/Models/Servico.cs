using System;
using System.Collections.Generic;

namespace Data.Entities.Models
{
    public partial class Servico
    {
        public Servico()
        {
            Agendamento = new HashSet<Agendamento>();
            PromocaoProdServ = new HashSet<PromocaoProdServ>();
            ServicoLog = new HashSet<ServicoLog>();
            ServicoProduto = new HashSet<ServicoProduto>();
            ServicoUsuario = new HashSet<ServicoUsuario>();
            VendaServico = new HashSet<VendaServico>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public TimeSpan? TempoEstimado { get; set; }
        public decimal Preco { get; set; }
        public decimal? PrecoAntigo { get; set; }
        public int RealizadoPorVet { get; set; }
        public int? PatazRecebido { get; set; }
        public int? PatazNecessario { get; set; }
        public int Ativo { get; set; }

        public virtual ICollection<Agendamento> Agendamento { get; set; }
        public virtual ICollection<PromocaoProdServ> PromocaoProdServ { get; set; }
        public virtual ICollection<ServicoLog> ServicoLog { get; set; }
        public virtual ICollection<ServicoProduto> ServicoProduto { get; set; }
        public virtual ICollection<ServicoUsuario> ServicoUsuario { get; set; }
        public virtual ICollection<VendaServico> VendaServico { get; set; }
    }
}
