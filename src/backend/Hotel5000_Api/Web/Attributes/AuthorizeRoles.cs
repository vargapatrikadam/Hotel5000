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
        public AuthorizeRoles(params Roles[] AllowedRoles)
        {
            this.AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
            var allowedRolesAsStrings = AllowedRoles.Select(x => Enum.GetName(typeof(Roles), x));
            this.Roles = string.Join(",", allowedRolesAsStrings);
        }
    }
}
