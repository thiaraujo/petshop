using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IBaseRepository<TEntity> : IDisposable where TEntity : class
    {
        Task AddAsync(TEntity entity);

        Task AddAllAsync(IEnumerable<TEntity> entity);

        void Update(TEntity entity);

        void Remove(TEntity entity);

        void RemoveAll(Expression<Func<TEntity, bool>> expression);

        Task<TEntity> GetByIdAsync(int id);

        Task<TEntity> GetByIdAsync(Expression<Func<TEntity, bool>> expression);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression);

        Task<TEntity> GetTopOneAsync();

        Task<TEntity> GetTopOneAsync(Expression<Func<TEntity, bool>> expression);

        Task<TEntity> GetLastOneAsync();

        Task<TEntity> GetLastOneAsync(Expression<Func<TEntity, bool>> expression);
    }
}
