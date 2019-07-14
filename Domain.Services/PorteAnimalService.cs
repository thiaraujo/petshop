using Data.Entities.Models;
using Domain.Interfaces;

namespace Domain.Services
{
    public class PorteAnimalService : BaseRepository<PorteAnimal>, IPorteAnimal
    {
        public PorteAnimalService(PetshopContext db) : base(db)
        {
        }
    }
}
