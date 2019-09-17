using Data.Entities.Models;
using Domain.Interfaces;

namespace Domain.Services
{
    public class ClientePontuacaoService : BaseRepository<ClientePontuacao>, IClientePontuacao
    {
        public ClientePontuacaoService(PetshopContext db) : base(db)
        {
        }
    }
}
