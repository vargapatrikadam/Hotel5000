using Ardalis.Specification;
using Core.Entities.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Specifications.Authorization
{
    public class GetOperationByAction : Specification<Operation>
    {
        public GetOperationByAction(Operation.Type actionType)
        {
            Query
                .Where(p => p.Action == actionType);
        }
    }
}
