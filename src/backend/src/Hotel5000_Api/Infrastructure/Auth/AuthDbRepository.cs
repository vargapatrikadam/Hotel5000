using Core.Entities;
using Infrastructure.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Auth
{
    public class AuthDbRepository<TEntity> : ARepository<TEntity, AuthDbContext>
        where TEntity : BaseEntity
    {
        public AuthDbRepository(AuthDbContext context) : base(context)
        {
        }
    }
}
