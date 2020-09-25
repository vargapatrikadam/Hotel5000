using Core.Enums.Authentication;
using System.Collections.Generic;

namespace Core.Entities.Domain
{
    public class Role : BaseEntity
    {
        public RoleType Name { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
