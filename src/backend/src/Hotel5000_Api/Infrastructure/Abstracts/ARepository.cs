using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Helpers;
using Microsoft.Data.SqlClient;
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
        protected bool IsInMemory { get; }
        protected string DatabaseName { get; }
        protected ARepository(TContext context)
        {
            Context = context;
            IsInMemory = Context.Database.IsInMemory();
            DatabaseName = Context.Database.GetDbConnection().Database;
        }

        public virtual async Task<int> AddAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
            await Context.SaveChangesAsync();
            return entity.Id;
        }

        public virtual async Task AddRange(IEnumerable<TEntity> entities)
        {
            await Context.Set<TEntity>().AddRangeAsync(entities);
            await Context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            var deleteThis = await Context.Set<TEntity>().FindAsync(entity.Id);
            Context.Set<TEntity>().Remove(deleteThis);
            await Context.SaveChangesAsync();
        }

        public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public virtual async Task<IReadOnlyList<TEntity>> GetAsync(ISpecification<TEntity> specification)
        {
            return await ApplySpecification(specification).AsNoTracking().ToListAsync();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            var data = Context.Set<TEntity>().Update(entity).Entity;
            Context.Entry(data).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().AnyAsync(predicate);
        }

        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification)
        {
            var evaluator = new SpecificationEvaluator<TEntity>();
            return evaluator.GetQuery(Context.Set<TEntity>().AsQueryable(), specification);
        }

        private async Task<TResult> CallStoredProcedure<TResult>(string procedureName, Dictionary<string, string> inputParameters, bool hasOutputValue, string outputParameter)
        {
            if (IsInMemory)
                throw new InvalidOperationException("Stored procedures cannot be invoked with an in-memory database provider.");

            if (hasOutputValue && outputParameter == null)
                throw new ArgumentException("Stored procedures that have return values need to specify a name for a return parameter when called");

            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            StringBuilder executeSql = new StringBuilder(procedureName);

            foreach (KeyValuePair<string, string> item in inputParameters)
            {
                sqlParameters.Add(new SqlParameter(item.Key, item.Value));
                executeSql.Append(@$" @{item.Key},");
            }

            if (hasOutputValue)
            {
                sqlParameters.Add(new SqlParameter(outputParameter, SqlHelper.GetDbType<TResult>())
                {
                    Direction = System.Data.ParameterDirection.Output
                });
                executeSql.Append(@$" @{outputParameter} OUTPUT,");
            }

            if (inputParameters.Count > 0 || hasOutputValue)
                //remove the comma from the last parameter
                executeSql.Length--;

            await Context.Database.ExecuteSqlRawAsync(executeSql.ToString(), sqlParameters);

            if (hasOutputValue)
            {
                SqlParameter sqlParameterWithOutputValue = sqlParameters.Where(p => p.ParameterName == outputParameter).FirstOrDefault();
                return (TResult)sqlParameterWithOutputValue.Value;
            }
            else return default(TResult);
        }
        protected virtual async Task<TResult> CallStoredProcedure<TResult>(string procedureName, Dictionary<string, string> inputParameters, string outputParameter)
        {
            return await CallStoredProcedure<TResult>(procedureName, inputParameters, true, outputParameter);
        }
        protected virtual async Task CallStoredProcedure(string procedureName, Dictionary<string, string> inputParameters)
        {
            await CallStoredProcedure<int>(procedureName, inputParameters, false, null);
        }

        public async Task<int> CountAsync()
        {
            return await Context.Set<TEntity>().CountAsync();
        }
    }
}