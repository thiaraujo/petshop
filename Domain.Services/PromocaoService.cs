using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Entities.Models;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services
{
    public class PromocaoService : BaseRepository<Promocao>, IPromocao
    {
        public PromocaoService(PetshopContext db) : base(db)
        {
        }

        public async Task<Promocao> CadastraOuAtualiza(Promocao promocao)
        {
            if (promocao.DataInicio.HasValue && promocao.DataFim.HasValue)
            {
                if (promocao.Id > 0)
                    Db.Update(promocao);
                else
                    await DbSet.AddAsync(promocao);

                await Db.SaveChangesAsync();

                return promocao;
            }

            return null;
        }

        public async Task<IEnumerable<Promocao>> ConsultaRegistros()
        {
            var promocoes = await DbSet.ToListAsync();
            return promocoes;
        }
    }
}
