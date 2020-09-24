using Ardalis.Specification;
using Core.Entities.Domain;
using Core.Enums.Lodging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Specifications.LodgingManagement
{
    public class GetLodgingType : Specification<LodgingType>
    {
        public GetLodgingType(LodgingTypes lodgingType)
        {
            Query
                .Where(p => p.Name == lodgingType);
        }
    }
}
