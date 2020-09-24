using Ardalis.Specification;
using Core.Entities.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Specifications.LodgingManagement
{
    public class GetCountry : Specification<Country>
    {
        public GetCountry(int? id = null, 
            string name = null, 
            string code = null)
        {
            Query
                .Where(p => (!id.HasValue || p.Id == id.Value) &&
                            (name == null || p.Name == name) &&
                            (code == null || p.Code == code));
        }
    }
}
