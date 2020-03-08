using Core.Entities;
using Infrastructure.Abstracts;

namespace Infrastructure.Logging
{
    public class LoggingDbRepository<TEntity> : ARepository<TEntity, LoggingDbContext>
        where TEntity : BaseEntity
    {
        public LoggingDbRepository(LoggingDbContext context) : base(context)
        {
        }
    }
}