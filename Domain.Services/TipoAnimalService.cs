using Data.Entities.Models;
using Domain.Interfaces;

namespace Domain.Services
{
    public class TipoAnimalService : BaseRepository<TipoAnimal>, ITipoAnimal
    {
        public TipoAnimalService(PetshopContext db) : base(db)
        {
        }
    }
}
