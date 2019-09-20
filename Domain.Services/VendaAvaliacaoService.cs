using Data.Entities.Models;
using Domain.Interfaces;

namespace Domain.Services
{
    public class VendaAvaliacaoService : BaseRepository<VendaAvaliacao>, IVendaAvaliacao
    {
        public VendaAvaliacaoService(PetshopContext db) : base(db)
        {
        }
    }
}
