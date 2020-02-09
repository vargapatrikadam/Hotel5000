using Core.Enums.Lodging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.LodgingEntities
{
    public class LodgingType : BaseEntity
    {
        public LodgingTypes Name { get; set; }

        public virtual ICollection<Lodging> Lodgings { get; set; }
    }
}
