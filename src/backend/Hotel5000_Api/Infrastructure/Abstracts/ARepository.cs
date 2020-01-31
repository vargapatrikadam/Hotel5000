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
        protected readonly TContext context;
        protected ARepository(TContext Context)
        {
            context = Context;
        }
        public async Task AddAsync(TEntity entity)
        {
            var newEntity = await context.Set<TEntity>().AddAsync(entity);
            await context.SaveChangesAsync();
            //TODO : is this necessary?
            context.DetachAllEntries();
        }
        public async Task DeleteAsync(TEntity entity)
        {
            TEntity deleteThis = await context.Set<TEntity>().FindAsync(entity.Id);
            context.Set<TEntity>().Remove(deleteThis);
            await context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
        {
            return await context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<IReadOnlyList<TEntity>> GetAsync(ISpecification<TEntity> specification)
        {
            return await ApplySpecification(specification).AsNoTracking().ToListAsync();
        }
        public async Task UpdateAsync(TEntity entity)
        {
            var data = context.Set<TEntity>().Update(entity).Entity;
            context.Entry(data).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification)
        {
            return SpecificationEvaluator<TEntity>.GetQuery(context.Set<TEntity>().AsQueryable(), specification);
        }
    }
}
