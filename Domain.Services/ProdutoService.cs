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
    public class ProdutoService : BaseRepository<Produto>, IProduto
    {
        public ProdutoService(PetshopContext db) : base(db)
        {
        }

        // Cadastra ou atualiza um registro
        public async Task<Produto> CadastraOuAtualiza(Produto produto)
        {
            if (string.IsNullOrEmpty(produto.Nome) || string.IsNullOrEmpty(produto.Fabricante))
                return null;

            if (produto.Id > 0)
                Db.Update(produto);
            else
            {
                produto.DataCadastro = DateTime.Now;
                await DbSet.AddAsync(produto);
            }

            await Db.SaveChangesAsync();
            return produto;
        }

        public async Task<IEnumerable<Produto>> ConsultaRegistros(string produto)
        {
            var produtos = await DbSet.ToListAsync();

            if (!string.IsNullOrEmpty(produto))
                produtos = produtos.Where(x => x.Nome.ToLower().Contains(produto.ToLower())).ToList();

            return produtos;
        }

        // Movimentações de baixa e entrada manual no estoque
        public async Task MovimentacaoEstoque(int produtoId, int? entrada, int? saida)
        {
            var produto = await DbSet.FindAsync(produtoId);
            if (produto == null)
                return;

            int? estoque;
            if (entrada.HasValue)
                estoque = +entrada;
            else
                estoque = -saida;

            produto.Estoque = estoque;
            Db.Update(produto);
            await Db.SaveChangesAsync();
        }

        public async Task DesabilitarRegistro(int produtoId)
        {
            var registro = await DbSet.FindAsync(produtoId);

            if (registro != null)
            {
                registro.Ativo = 0;

                Db.Update(registro);
                await Db.SaveChangesAsync();
            }
        }

        public async Task<ProdutoViewModel> RegistroDoProduto(int produtoId)
        {
            var produto = await DbSet.FindAsync(produtoId);
            var promocao = await Db.PromocaoProdServ
                .Include(x => x.Promocao)
                .FirstOrDefaultAsync(x =>
                x.ProdutoId == produtoId && x.Promocao.DataInicio <= DateTime.Now && x.Promocao.DataFim >= DateTime.Now);

            //Se tiver promoção, calcula o valor
            decimal valorComDesconto = 0;
            double descontoAplicado = 0;
            if (promocao != null)
            {
                valorComDesconto = ((produto.Preco ?? 0) - ((produto.Preco ?? 0) * (promocao.Promocao.Percentual ?? 0)) / 100);
                descontoAplicado = Convert.ToDouble(promocao.Promocao.Percentual);
            }

            var produtoFinal = new ProdutoViewModel
            {
                ProdutoId = produtoId,
                Preco = produto.Preco,
                PrecoComDesconto = valorComDesconto,
                PercentualDesconto = descontoAplicado
            };

            return produtoFinal;
        }

        public async Task<bool> AtualizaEstoque(int id, int estoque)
        {
            var registro = await DbSet.FindAsync(id);
            if (registro == null) return false;

            registro.Estoque += estoque;
            Db.Update(registro);
            await Db.SaveChangesAsync();
            return true;
        }
    }
}
