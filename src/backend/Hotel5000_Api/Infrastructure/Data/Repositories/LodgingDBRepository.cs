using Core.Entities;
using Infrastructure.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Repositories
{
    public class LodgingDBRepository<TEntity> : ARepository<TEntity, LodgingDBContext> 
        where TEntity : BaseEntity
    {
        public LodgingDBRepository(LodgingDBContext context) : base(context)
        {

        }
    }
}
