using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ApplicationRepository<T> : IAsyncRepository<T> where T : BaseEntity
    {
        protected readonly DbContext context;
        public ApplicationRepository(DbContext Context)
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

        public Task<IReadOnlyList<T>> GetAsync(ISpecification<T> specification)
        {
            throw new NotImplementedException();
        }

        public async Task<T> UpdateAsync(T entity)
        {
            var data = context.Set<T>().Update(entity).Entity;
            context.Entry(data).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return data;
        }
    }
}
