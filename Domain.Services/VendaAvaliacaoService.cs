using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Entities.Models;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services
{
    public class VendaAvaliacaoService : BaseRepository<VendaAvaliacao>, IVendaAvaliacao
    {
        public VendaAvaliacaoService(PetshopContext db) : base(db)
        {
        }

        async Task<IEnumerable<VendaAvaliacao>> IBaseRepository<VendaAvaliacao>.GetAllAsync(Expression<Func<VendaAvaliacao, bool>> expression)
        {
            return await DbSet
                .Include(x => x.Agendamento)
                .Include(x => x.Agendamento.Servico)
                .Include(x => x.Agendamento.Cliente)
                .Include(x => x.Agendamento.Usuario)
                .Where(expression)
                .ToListAsync();
        }
    }
}
