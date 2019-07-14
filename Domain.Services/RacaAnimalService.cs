using Data.Entities.Models;
using Domain.Interfaces;

namespace Domain.Services
{
    public class RacaAnimalService : BaseRepository<RacaAnimal>, IRacaAnimal
    {
        public RacaAnimalService(PetshopContext db) : base(db)
        {
        }
    }
}
