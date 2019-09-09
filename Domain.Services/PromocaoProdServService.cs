using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Entities.Models;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services
{
    public class PromocaoProdServService : BaseRepository<PromocaoProdServ>, IPromocaoProdServ
    {
        public PromocaoProdServService(PetshopContext db) : base(db)
        {
        }

        async Task<PromocaoProdServ> IBaseRepository<PromocaoProdServ>.GetByIdAsync(Expression<Func<PromocaoProdServ, bool>> expression)
        {
            return await DbSet
                .Include(x => x.Promocao)
                .FirstOrDefaultAsync(expression);
        }



        async Task<PromocaoProdServ> IBaseRepository<PromocaoProdServ>.GetByIdAsync(int id)
        {
            return await DbSet
                .Include(x => x.Promocao)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<PromocaoProdServ>> ConsultaRegistros(int promocaoId)
        {
            var produtosNaPromocao = await DbSet.Where(x => x.PromocaoId == promocaoId).ToListAsync();
            return produtosNaPromocao;
        }

        public async Task CadastraOuAtualizaProdServNaPromocao(int promocaoId, int[] produtos, int[] servicos)
        {
            if (!produtos.Any() && !servicos.Any())
                return;

            var jaRegistrados = await DbSet.Where(x => x.PromocaoId == promocaoId).ToListAsync();

            if (jaRegistrados.Any())
            {
                Db.RemoveRange(jaRegistrados);
            }

            //Adiciona as especialidades
            var prods = new List<PromocaoProdServ>();
            foreach (var item in produtos)
            {
                prods.Add(new PromocaoProdServ
                {
                    ProdutoId = item,
                    PromocaoId = promocaoId
                });
            }

            foreach (var item in servicos)
            {
                prods.Add(new PromocaoProdServ
                {
                    ServicoId = item,
                    PromocaoId = promocaoId
                });
            }

            await Db.AddRangeAsync(prods);
            await Db.SaveChangesAsync();
        }
    }
}
