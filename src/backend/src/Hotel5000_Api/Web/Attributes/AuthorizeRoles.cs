using Core.Enums.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;

namespace Web.Attributes
{
    public class AuthorizeRoles : AuthorizeAttribute
    {
        private static RoleType[] RolesWithoutAnonymous = new RoleType[] { RoleType.ADMIN, RoleType.APPROVED_USER, RoleType.COMPANY };
        private static RoleType[] RolesWithAnonymous = new RoleType[] { RoleType.ADMIN, RoleType.APPROVED_USER, RoleType.COMPANY, RoleType.ANONYMOUS };
        public AuthorizeRoles(bool allowAnonymous = false) : this(allowAnonymous ? RolesWithAnonymous : RolesWithoutAnonymous)
        {

        }
        public AuthorizeRoles(params RoleType[] allowedRoles)
        {
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
            var allowedRolesAsStrings = allowedRoles.Select(x => Enum.GetName(typeof(RoleType), x));
            Roles = string.Join(",", allowedRolesAsStrings);
        }
    }
}