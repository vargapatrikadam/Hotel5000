using Core.Enums.Lodging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Attributes
{
    public class AuthorizeRoles : AuthorizeAttribute
    {
        public AuthorizeRoles(params Roles[] allowedRoles)
        {
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
            var allowedRolesAsStrings = allowedRoles.Select(x => Enum.GetName(typeof(Roles), x));
            Roles = string.Join(",", allowedRolesAsStrings);
        }
    }
}