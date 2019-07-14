using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Entities.Models;

namespace Domain.Interfaces
{
    public interface IAnimal : IBaseRepository<Animal>
    {
        Task<Animal> CadastraOuAtualiza(Animal animal);
        Task<IEnumerable<Animal>> ConsultaRegistros(string animal);
    }
}
