using Core.Enums.Lodging;
using System.Collections.Generic;

namespace Core.Entities.LodgingEntities
{
    public class LodgingType : BaseEntity
    {
        public LodgingTypes Name { get; set; }

        public ICollection<Lodging> Lodgings { get; set; }
    }
}
