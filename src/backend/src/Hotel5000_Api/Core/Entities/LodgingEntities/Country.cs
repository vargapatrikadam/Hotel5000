using System.Collections.Generic;

namespace Core.Entities.LodgingEntities
{
    public class Country : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<LodgingAddress> LodgingAddresses { get; set; }
    }
}