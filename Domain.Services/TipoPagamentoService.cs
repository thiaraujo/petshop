using Data.Entities.Models;
using Domain.Interfaces;

namespace Domain.Services
{
    public class TipoPagamentoService : BaseRepository<TipoPagamento>, ITipoPagamento
    {
        public TipoPagamentoService(PetshopContext db) : base(db)
        {
        }
    }
}
