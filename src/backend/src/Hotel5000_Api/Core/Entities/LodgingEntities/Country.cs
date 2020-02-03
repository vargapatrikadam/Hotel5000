using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.LodgingEntities
{
    public class Country : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<LodgingAddress> LodgingAddresses { get; set; }
    }
}