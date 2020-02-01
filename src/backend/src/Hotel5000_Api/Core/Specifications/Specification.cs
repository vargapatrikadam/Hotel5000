using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.Specifications
{
    public class Specification<TEntity> : ISpecification<TEntity> where TEntity : class
    {
        public Specification()
        {
            Includes = new List<Expression<Func<TEntity, object>>>();
            IsPagingEnabled = false;
        }
        public Expression<Func<TEntity, bool>> Criteria { get; private set;  }
        public List<Expression<Func<TEntity, object>>> Includes { get; private set; }
        public Expression<Func<TEntity, object>> OrderBy { get; private set; }
        public Expression<Func<TEntity, object>> OrderByDescending { get; private set; }
        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool IsPagingEnabled { get; private set; }

        public ISpecification<TEntity> ApplyFilter(Expression<Func<TEntity, bool>> filter)
        {
            Criteria = filter;
            return this;
        }
        public ISpecification<TEntity> AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            Includes.Add(includeExpression);
            return this;
        }
        public ISpecification<TEntity> ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
            return this;
        }
        public ISpecification<TEntity> ApplyOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
            return this;
        }
        public ISpecification<TEntity> ApplyOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
            return this;
        }

    }
}
