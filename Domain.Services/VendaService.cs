using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities.Models;
using Data.Entities.ViewModels;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services
{
    public class VendaService : BaseRepository<Venda>, IVenda
    {
        public VendaService(PetshopContext db) : base(db)
        {
        }

        // Após ser efetuado o agendamento, o operador tem a possibilidade de gerar uma venda
        public async Task<Venda> GerarVendaAposAgendamento(Agendamento agendamento)
        {
            var venda = new Venda
            {
                AgendamentoId = agendamento.Id
            };

            await DbSet.AddAsync(venda);
            await Db.SaveChangesAsync();

            return venda;
        }

        public async Task RegistraOpcaoPagamento(int pagamentoId, int vendaId)
        {
            var registro = await DbSet.FindAsync(vendaId);

            if (registro == null) return;

            registro.TipoPagamentoId = pagamentoId;

            Db.Update(registro);
            await Db.SaveChangesAsync();
        }

        public async Task RegistraVendaProduto(IEnumerable<ProdutoViewModel> produtos, int agendamentoId)
        {
            var registro = await Db.Agendamento.FindAsync(agendamentoId);

            if (registro == null) return;

            var vendaProdutos = new List<VendaProduto>();
            foreach (var item in produtos)
            {
                vendaProdutos.Add(new VendaProduto
                {
                    ProdutoId = item.ProdutoId,
                    Valor = item.Preco ?? 0,
                    ValorComDesconto = item.PrecoComDesconto ?? 0,
                });
            }

            await Db.VendaProduto.AddRangeAsync(vendaProdutos);
            await Db.SaveChangesAsync();
        }

        public async Task RegistraVendaServico(IEnumerable<ServicoViewModel> servicos, int agendamentoId)
        {
            var registro = await Db.Agendamento.FindAsync(agendamentoId);

            if (registro == null) return;

            var vendaServicos = new List<VendaServico>();
            foreach (var item in servicos)
            {
                vendaServicos.Add(new VendaServico
                {
                    ServicoId = item.ServicoId,
                    ValorComDesconto = item.PrecoComDesconto,
                    Valor = item.Preco ?? 0
                });
            }

            await Db.VendaServico.AddRangeAsync(vendaServicos);
            await Db.SaveChangesAsync();
        }

        // Se chegar aqui, faz as baixas no estoque
        public async Task<string> ConcretizaVenda(decimal valorPago, int? pontosUtilizados, int vendaId)
        {
            var registro = await DbSet.FindAsync(vendaId);

            if (registro == null) return null;

            //Se o pagamento for com pataz, verifica se o cliente pode prosseguir
            var pataz = await CalculaPatazRecebidoOuNecessario(registro.AgendamentoId);
            if (pontosUtilizados.HasValue && pontosUtilizados.Value > 0)
            {
                if (pontosUtilizados < pataz)
                    return "Cliente não possui saldo suficiente no programa Pataz!";
            }

            registro.ValorPago = valorPago;
            registro.ValorProdutos = await ValorEmProdutos(registro.AgendamentoId);
            registro.ValorProdutosDesconto = await ValorEmProdutosComDesconto(registro.AgendamentoId);
            registro.ValorServico = await ValorEmServicos(registro.AgendamentoId);
            registro.ValorServicoDesconto = await ValorEmServicosComDesconto(registro.AgendamentoId);
            registro.PatazTotalRecebido = pataz;

            Db.Update(registro);
            await Db.SaveChangesAsync();

            //Baixa no estoque
            await BaixaNoEstoque(vendaId);

            return "Venda realizada com sucesso!";
        }

        private async Task<decimal> ValorEmProdutos(int agendamentoid)
        {
            var vendidos = await Db.VendaProduto.Where(x => x.AgendamentoId == agendamentoid).ToListAsync();
            if (vendidos.Any())
            {
                var valores = vendidos.Where(x => !x.ValorComDesconto.HasValue).Sum(x => x.Valor);

                return valores;
            }

            return 0;
        }

        private async Task<decimal> ValorEmProdutosComDesconto(int agendamentoid)
        {
            var vendidos = await Db.VendaProduto.Where(x => x.AgendamentoId == agendamentoid).ToListAsync();
            if (vendidos.Any())
            {
                var valoresComDesconto = vendidos.Where(x => x.ValorComDesconto.HasValue).Sum(x => x.ValorComDesconto);

                return valoresComDesconto ?? 0;
            }

            return 0;
        }

        private async Task<decimal> ValorEmServicos(int agendamentoid)
        {
            var vendidos = await Db.VendaServico.Where(x => x.AgendamentoId == agendamentoid).ToListAsync();
            if (vendidos.Any())
            {
                var valores = vendidos.Where(x => !x.ValorComDesconto.HasValue).Sum(x => x.Valor);

                return valores;
            }

            return 0;
        }

        private async Task<decimal> ValorEmServicosComDesconto(int agendamentoid)
        {
            var vendidos = await Db.VendaServico.Where(x => x.AgendamentoId == agendamentoid).ToListAsync();
            if (vendidos.Any())
            {
                var valoresComDesconto = vendidos.Where(x => x.ValorComDesconto.HasValue).Sum(x => x.ValorComDesconto);

                return valoresComDesconto ?? 0;
            }

            return 0;
        }

        private async Task<int> CalculaPatazRecebidoOuNecessario(int agendamentoid)
        {
            var servicos = await Db.VendaServico
                .Include(x => x.Servico)
                .Where(x => x.AgendamentoId == agendamentoid).ToListAsync();

            var pataz = servicos.Sum(x => x.Servico.PatazRecebido);
            return pataz ?? 0;
        }

        private async Task BaixaNoEstoque(int agendamentoid)
        {
            var vendidos = await Db.VendaProduto.Where(x => x.AgendamentoId == agendamentoid).ToListAsync();
            if (vendidos.Any())
            {
                var produtosVendidos = await Db.Produto.Where(x => vendidos.Any(v => v.ProdutoId == x.Id)).ToListAsync();
                foreach (var item in produtosVendidos)
                {
                    var estoque = vendidos.FirstOrDefault(x => x.ProdutoId == item.Id);
                    if (estoque != null)
                    {
                        item.Estoque = item.Estoque - estoque.Quantidade ?? 1;
                    }
                }
                Db.Produto.UpdateRange(produtosVendidos);
                await Db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Venda>> ConsultaRegistros()
        {
            var vendas = await DbSet
                .Include(x => x.Agendamento)
                .Include(x => x.TipoPagamento)
                .Include(x => x.Agendamento.VendaProduto)
                .ToListAsync();

            return vendas;
        }
    }
}
