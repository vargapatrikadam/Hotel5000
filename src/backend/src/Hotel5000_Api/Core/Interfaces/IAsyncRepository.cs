using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAsyncRepository<TEntity> where TEntity : class
    {
        Task<IReadOnlyList<TEntity>> GetAllAsync();
        Task<IReadOnlyList<TEntity>> GetAsync(ISpecification<TEntity> specification);
        Task AddAsync(TEntity entity);
        Task AddRange(IEnumerable<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

    }
}