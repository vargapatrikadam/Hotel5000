using Core.Entities;
using Infrastructure.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Lodgings
{
    public class LodgingDbRepository<TEntity> : ARepository<TEntity, LodgingDbContext>
        where TEntity : BaseEntity
    {
        public LodgingDbRepository(LodgingDbContext context) : base(context)
        {
        }
    }
}