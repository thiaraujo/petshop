﻿using System;
using System.Collections.Generic;

namespace Data.Entities.Models
{
    public partial class Venda
    {
        public Venda()
        {
            VendaAvaliacao = new HashSet<VendaAvaliacao>();
            VendaProduto = new HashSet<VendaProduto>();
            VendaServico = new HashSet<VendaServico>();
        }

        public int Id { get; set; }
        public int AgendamentoId { get; set; }
        public int TipoPagamentoId { get; set; }
        public decimal? ValorProdutos { get; set; }
        public decimal? ValorProdutosDesconto { get; set; }
        public decimal? ValorServico { get; set; }
        public decimal? ValorServicoDesconto { get; set; }
        public decimal ValorPago { get; set; }
        public int? PatazTotalRecebido { get; set; }
        public DateTime DataPagamento { get; set; }

        public virtual Agendamento Agendamento { get; set; }
        public virtual TipoPagamento TipoPagamento { get; set; }
        public virtual ICollection<VendaAvaliacao> VendaAvaliacao { get; set; }
        public virtual ICollection<VendaProduto> VendaProduto { get; set; }
        public virtual ICollection<VendaServico> VendaServico { get; set; }
    }
}
