using Core.Entities;
using Infrastructure.Abstracts;

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