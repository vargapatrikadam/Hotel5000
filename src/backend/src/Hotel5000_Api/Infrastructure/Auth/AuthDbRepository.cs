using Core.Entities;
using Infrastructure.Abstracts;

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
