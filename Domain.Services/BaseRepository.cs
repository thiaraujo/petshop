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
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        public PetshopContext Db;
        public DbSet<TEntity> DbSet;

        public BaseRepository(PetshopContext db)
        {
            Db = db;
            DbSet = Db.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
            await Db.SaveChangesAsync();
        }

        public async Task AddAllAsync(IEnumerable<TEntity> entity)
        {
            foreach (var item in entity) DbSet.Add(item);
            await Db.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            DbSet.Attach(entity);
            Db.Entry(entity).State = EntityState.Modified;
            Db.SaveChanges();
        }

        public void Remove(TEntity entity)
        {
            DbSet.Remove(entity);
            Db.SaveChanges();
        }

        public void RemoveAll(Expression<Func<TEntity, bool>> expression)
        {
            IQueryable<TEntity> query = DbSet;
            var listDelete = query.Where(expression).ToList();

            foreach (var item in listDelete) DbSet.Remove(item);
            Db.SaveChanges();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<TEntity> GetByIdAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await DbSet.FirstOrDefaultAsync(expression);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await DbSet.Where(expression).ToListAsync();
        }

        public async Task<TEntity> GetTopOneAsync()
        {
            return await DbSet.FirstOrDefaultAsync();
        }

        public async Task<TEntity> GetTopOneAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await DbSet.FirstOrDefaultAsync(expression);
        }

        public async Task<TEntity> GetLastOneAsync()
        {
            return await DbSet.LastOrDefaultAsync();
        }

        public async Task<TEntity> GetLastOneAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await DbSet.LastOrDefaultAsync(expression);
        }

        public void Dispose()
        {
            DbSet = null;
            Db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
