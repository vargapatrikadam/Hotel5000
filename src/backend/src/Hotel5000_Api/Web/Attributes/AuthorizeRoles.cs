using Core.Enums.Lodging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;

namespace Web.Attributes
{
    public class AuthorizeRoles : AuthorizeAttribute
    {
        public AuthorizeRoles() : this(new Roles[] { Core.Enums.Lodging.Roles.Company, Core.Enums.Lodging.Roles.ApprovedUser, Core.Enums.Lodging.Roles.Admin })
        {

        }
        public AuthorizeRoles(params Roles[] allowedRoles)
        {
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
            var allowedRolesAsStrings = allowedRoles.Select(x => Enum.GetName(typeof(Roles), x));
            Roles = string.Join(",", allowedRolesAsStrings);
        }
    }
}