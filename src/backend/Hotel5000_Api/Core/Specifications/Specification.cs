using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.Specifications
{
    public class Specification<T> : ISpecification<T> where T : class
    {
        public Specification()
        {
            Includes = new List<Expression<Func<T, object>>>();
            IsPagingEnabled = false;
        }
        public Expression<Func<T, bool>> Criteria { get; private set;  }
        public List<Expression<Func<T, object>>> Includes { get; private set;  }
        public Expression<Func<T, object>> OrderBy { get; private set; }
        public Expression<Func<T, object>> OrderByDescending { get; private set; }
        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool IsPagingEnabled { get; private set; }

        public Specification<T> ApplyFilter(Expression<Func<T, bool>> filter)
        {
            Criteria = filter;
            return this;
        }
        public Specification<T> Include(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
            return this;
        }
        public Specification<T> ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
            return this;
        }
        public Specification<T> ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
            return this;
        }
        public Specification<T> ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
            return this;
        }

    }
}
