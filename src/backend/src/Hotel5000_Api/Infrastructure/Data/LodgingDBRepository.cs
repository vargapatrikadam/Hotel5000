using Core.Entities;
using Infrastructure.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data
{
    public class LodgingDBRepository<TEntity> : ARepository<TEntity, LodgingDBContext> 
        where TEntity : BaseEntity
    {
        public LodgingDBRepository(LodgingDBContext context) : base(context)
        {

        }
    }
}
