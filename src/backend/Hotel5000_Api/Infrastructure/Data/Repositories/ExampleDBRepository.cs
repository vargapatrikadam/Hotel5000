using Core.Entities;
using Infrastructure.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Repositories
{
    public class ExampleDBRepository<TEntity> : ARepository<TEntity, ExampleDBContext>
        where TEntity : BaseEntity
    {
        public ExampleDBRepository(ExampleDBContext Context) : base(Context)
        {
        }
    }
}
