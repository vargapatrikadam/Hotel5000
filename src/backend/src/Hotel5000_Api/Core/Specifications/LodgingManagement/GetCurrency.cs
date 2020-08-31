using Ardalis.Specification;
using Core.Entities.LodgingEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications.LodgingManagement
{
    public class GetCurrency : Specification<Currency>
    {
        public GetCurrency(int? id = null, 
            string name = null)
        {
            Query
                .Where(p => (!id.HasValue || p.Id == id.Value) &&
                            (name == null || p.Name == name));
        }
    }
}
