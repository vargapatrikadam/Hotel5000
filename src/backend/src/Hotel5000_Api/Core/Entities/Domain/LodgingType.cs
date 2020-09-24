using Core.Enums.Lodging;
using System.Collections.Generic;

namespace Core.Entities.Domain
{
    public class LodgingType : BaseEntity
    {
        public LodgingTypes Name { get; set; }

        public ICollection<Lodging> Lodgings { get; set; }
    }
}
