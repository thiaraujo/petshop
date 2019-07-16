using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities.Models;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services
{
    public class UsuarioEspecialidadeService : BaseRepository<UsuarioEspecialidade>, IUsuarioEspecialidade
    {
        public UsuarioEspecialidadeService(PetshopContext db) : base(db)
        {
        }

        public async Task CadastraOuAtualizaEspecialidade(int usuarioId, int[] tipos)
        {
            if (!tipos.Any())
                return;

            var jaRegistrados = await DbSet.Where(x => x.UsuarioId == usuarioId).ToListAsync();

            if (jaRegistrados.Any())
            {
                Db.RemoveRange(jaRegistrados);
            }

            //Adiciona as especialidades
            var especialidades = new List<UsuarioEspecialidade>();
            foreach (var item in tipos)
            {
                especialidades.Add(new UsuarioEspecialidade
                {
                    UsuarioId = usuarioId,
                    TipoAnimalId = item
                });
            }

            await Db.AddRangeAsync(especialidades);
            await Db.SaveChangesAsync();
        }

        public async Task<IEnumerable<UsuarioEspecialidade>> ConsultaEspecialidadesUsuario(int usuarioId)
        {
            return await DbSet
                .Include(x => x.TipoAnimal)
                .Where(x => x.UsuarioId == usuarioId)
                .ToListAsync();
        }
    }
}
