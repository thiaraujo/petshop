using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities.Models;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services
{
    public class ServicoProdutoService : BaseRepository<ServicoProduto>, IServicoProduto
    {
        public ServicoProdutoService(PetshopContext db) : base(db)
        {
        }

        public async Task CadastraOuAtualizaProdutosNoServico(int servicoId, int[] produtos)
        {
            if (!produtos.Any())
                return;

            var jaRegistrados = await DbSet.Where(x => x.ServicoId == servicoId).ToListAsync();

            if (jaRegistrados.Any())
            {
                Db.RemoveRange(jaRegistrados);
            }

            //Adiciona as especialidades
            var prodServico = new List<ServicoProduto>();
            foreach (var item in produtos)
            {
                prodServico.Add(new ServicoProduto
                {
                    ServicoId = servicoId,
                    ProdutoId = item
                });
            }

            await Db.AddRangeAsync(prodServico);
            await Db.SaveChangesAsync();
        }

        public async Task<IEnumerable<ServicoProduto>> ConsultaProdutosDoServico(int servicoId)
        {
            return await DbSet
                .Include(x => x.Produto)
                .Include(x => x.Servico)
                .Where(x => x.ServicoId == servicoId)
                .ToListAsync();
        }
    }
}
