using Core.Enums.Authentication;
using System.Collections.Generic;

namespace Core.Entities.Authentication
{
    public class BaseRole : BaseEntity
    {
        public bool CanEditOthers { get; set; }
        public RoleType Name { get; set; }
        public ICollection<Rule> Rules { get; set; }
    }
}
