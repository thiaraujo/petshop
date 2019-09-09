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
    public class VendaProdutoService : BaseRepository<VendaProduto>, IVendaProduto
    {
        public VendaProdutoService(PetshopContext db) : base(db)
        {
        }

        async Task<IEnumerable<VendaProduto>> IBaseRepository<VendaProduto>.GetAllAsync(Expression<Func<VendaProduto, bool>> expression)
        {
            return await DbSet
                .Include(x => x.Produto)
                .Include(x => x.Agendamento)
                .Where(expression)
                .ToListAsync();
        }
    }
}
