using Core.Entities;
using Core.Entities.Domain;
using Infrastructure.Abstracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Lodgings
{
    public class LodgingDbRepository<TEntity> : ARepository<TEntity, LodgingDbContext>
        where TEntity : BaseEntity
    {
        public LodgingDbRepository(LodgingDbContext context) : base(context)
        {
        }
        public override Task<int> AddAsync(TEntity entity)
        {
            if (!IsInMemory)
            {
                switch (entity)
                {
                    case User _:
                        return AddAsync(entity as User);
                    default:
                        return base.AddAsync(entity);
                }
            }
            return base.AddAsync(entity);
        }
        private Task<int> AddAsync(User entity)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add(nameof(entity.Username), entity.Username);
            parameters.Add(nameof(entity.Password), entity.Password);
            parameters.Add(nameof(entity.Email), entity.Email);
            parameters.Add(nameof(entity.FirstName), entity.FirstName);
            parameters.Add(nameof(entity.LastName), entity.LastName);
            parameters.Add(nameof(entity.RoleId), entity.RoleId.ToString());
            return base.CallStoredProcedure<int>("AddUser", parameters, "ReturnValue");
        }
    }
}