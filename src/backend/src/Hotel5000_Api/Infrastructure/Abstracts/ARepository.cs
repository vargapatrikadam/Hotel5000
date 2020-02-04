using Core.Entities;
using Core.Interfaces;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Abstracts
{
    public abstract class ARepository<TEntity, TContext> : IAsyncRepository<TEntity> where TEntity : BaseEntity
        where TContext : DbContext
    {
        protected readonly TContext Context;

        protected ARepository(TContext context)
        {
            Context = context;
        }

        public async Task AddAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
            await Context.SaveChangesAsync();
            //TODO : is this necessary?
            //Context.DetachAllEntries();
        }

        public async Task AddRange(IEnumerable<TEntity> entities)
        {
            await Context.Set<TEntity>().AddRangeAsync(entities);
            await Context.SaveChangesAsync();
            //TODO : is this necessary?
            //Context.DetachAllEntries();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            var deleteThis = await Context.Set<TEntity>().FindAsync(entity.Id);
            Context.Set<TEntity>().Remove(deleteThis);
            await Context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<IReadOnlyList<TEntity>> GetAsync(ISpecification<TEntity> specification)
        {
            return await ApplySpecification(specification).AsNoTracking().ToListAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            var data = Context.Set<TEntity>().Update(entity).Entity;
            Context.Entry(data).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }

        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification)
        {
            return SpecificationEvaluator<TEntity>.GetQuery(Context.Set<TEntity>().AsQueryable(), specification);
        }
    }
}