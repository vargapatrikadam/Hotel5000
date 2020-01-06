using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class LoggingDBRepository<T> : IAsyncRepository<T> where T : BaseEntity
    {
        protected readonly LoggingDBContext context;
        public LoggingDBRepository(LoggingDBContext Context)
        {
            context = Context;
        }
        public async Task<T> AddAsync(T entity)
        {
            var newEntity = await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
            return newEntity.Entity;
        }

        public async Task DeleteAsync(T entity)
        {
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).AsNoTracking().ToListAsync();
        }

        public async Task<T> UpdateAsync(T entity)
        {
            var data = context.Set<T>().Update(entity).Entity;
            context.Entry(data).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return data;
        }
        private IQueryable<T> ApplySpecification(ISpecification<T> specification)
        {
            return SpecificationEvaluator<T>.GetQuery(context.Set<T>().AsQueryable(), specification);
        }
    }
}
