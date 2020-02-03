using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.Interfaces
{
    public interface ISpecification<TEntity> where TEntity : class
    {
        Expression<Func<TEntity, bool>> Criteria { get; }
        List<Expression<Func<TEntity, object>>> Includes { get; }
        Expression<Func<TEntity, object>> OrderBy { get; }
        Expression<Func<TEntity, object>> OrderByDescending { get; }

        int Take { get; }
        int Skip { get; }
        bool IsPagingEnabled { get; }

        public ISpecification<TEntity> ApplyFilter(Expression<Func<TEntity, bool>> filter);
        public ISpecification<TEntity> AddInclude(Expression<Func<TEntity, object>> includeExpression);
        public ISpecification<TEntity> ApplyPaging(int skip, int take);
        public ISpecification<TEntity> ApplyOrderBy(Expression<Func<TEntity, object>> orderByExpression);

        public ISpecification<TEntity> ApplyOrderByDescending(
            Expression<Func<TEntity, object>> orderByDescendingExpression);
    }
}