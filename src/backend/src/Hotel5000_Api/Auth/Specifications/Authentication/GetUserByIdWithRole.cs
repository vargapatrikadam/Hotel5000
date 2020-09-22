﻿using Ardalis.Specification;
using Core.Entities.LodgingEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Specifications.Authentication
{ 
    class GetUserByIdWithRole : Specification<User>
    {
        public GetUserByIdWithRole(int userId)
        {
            Query
                .Where(p => p.Id == userId)
                .Include(i => i.Role);
        }
    }
}