using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities.Models;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services
{
    public class VendaService : BaseRepository<Venda>, IVenda
    {
        public VendaService(PetshopContext db) : base(db)
        {
        }

        // Registra o produto selecionado para venda no agendamento
        public async Task RegistraVendaProduto(VendaProduto produto)
        {
            await Db.VendaProduto.AddAsync(produto);
            await Db.SaveChangesAsync();
        }

        // Se chegar aqui, faz as baixas no estoque
        public async Task<string> ConcretizaVenda(Venda venda)
        {
            await DbSet.AddAsync(venda);
            await Db.SaveChangesAsync();
            //Baixa no estoque
            await BaixaNoEstoque(venda.AgendamentoId);
            //Atualiza a pontuação
            await AtualizaPontosPetz(venda);

            return "Venda realizada com sucesso!";
        }

        // Atualiza o saldo de pontos petz do cliente
        public async Task AtualizaPontosPetz(Venda venda)
        {
            var vendaCompleta = await DbSet
                .Include(x => x.Agendamento.Servico)
                .Include(x => x.Agendamento.Cliente)
                .FirstOrDefaultAsync(x => x.Id == venda.Id);
            var cliente = await Db.Cliente.FindAsync(vendaCompleta.Agendamento.ClienteId);

            // Pega a pontuação atual
            var pontuacao = await Db.ClientePontuacao.FindAsync(cliente.Id);

            // Se ainda não possuir registro de pontuação, então cria
            if (pontuacao == null)
            {
                var ponto = new ClientePontuacao
                {
                    ClienteId = cliente.Id,
                    DataAtualizado = DateTime.Now,
                    Pontos = vendaCompleta.Agendamento.Servico.PatazRecebido
                };

                await Db.ClientePontuacao.AddAsync(ponto);
            }
            else
            {
                // Adiciona os pontos recebidos
                pontuacao.Pontos = pontuacao.Pontos + vendaCompleta.Agendamento.Servico.PatazRecebido;
                pontuacao.DataAtualizado = DateTime.Now;

                //se for tipo de venda 4, onde é o pagamento por pontos, então debita -> se chegou aqui, é porque tinha o necessário
                if (venda.TipoPagamentoId == 4)
                {
                    var necessario = venda.Agendamento.Servico.PatazNecessario ?? 0;
                    pontuacao.Pontos = pontuacao.Pontos - necessario;
                }

                Db.ClientePontuacao.Update(pontuacao);
            }

            await Db.SaveChangesAsync();
        }

        // Ao finalizar a venda, se tiver produto, faz a baixa no estoque
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

        // Consulta os registros, incluindos suas dependencias
        public async Task<IEnumerable<Venda>> ConsultaRegistros()
        {
            var vendas = await DbSet
                .Include(x => x.Agendamento)
                .Include(x => x.Agendamento.Servico)
                .Include(x => x.Agendamento.Cliente)
                .Include(x => x.Agendamento.Animal)
                .Include(x => x.TipoPagamento)
                .Include(x => x.Agendamento.VendaProduto)
                .ToListAsync();

            return vendas;
        }
    }
}
