using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities.Models;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services
{
    public class AnimalService : BaseRepository<Animal>, IAnimal
    {
        public AnimalService(PetshopContext db) : base(db)
        {
        }

        // Função padrão para atualizar ou cadastrar um registro
        public async Task<Animal> CadastraOuAtualiza(Animal animal)
        {
            if (!animal.TipoAnimalId.HasValue || string.IsNullOrEmpty(animal.Nome) || !animal.RacaAnimalId.HasValue)
                return null;

            if (animal.Id > 0)
                Db.Update(animal);
            else
                await DbSet.AddAsync(animal);

            await Db.SaveChangesAsync();
            return animal;
        }

        // Função padrão para consulta de registros
        public async Task<IEnumerable<Animal>> ConsultaRegistros(string animal)
        {
            var animais = await DbSet
                .Include(x => x.Cliente)
                .Include(x => x.PorteAnimal)
                .Include(x => x.RacaAnimal)
                .Include(x => x.TipoAnimal)
                .ToListAsync();

            if (!string.IsNullOrEmpty(animal))
                animais = animais.Where(x => x.Nome.ToUpper().Contains(animal.ToLower())).ToList();

            return animais;
        }
    }
}
