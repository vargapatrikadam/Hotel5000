using Ardalis.Specification;
using Core.Entities.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Specifications.Authorization
{
    class GetEntityByName : Specification<Entity>
    {
        public GetEntityByName(string name)
        {
            Query
                .Where(p => p.Name == name);
        }
    }
}
