using Microsoft.AspNetCore.Authorization;
using RO.DevTest.Domain.Enums;

namespace RO.DevTest.WebApi.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeRolesAttribute : AuthorizeAttribute
{
    public AuthorizeRolesAttribute(params UserRoles[] roles)
    {
        Roles = string.Join(",", roles.Select(r => r.ToString()));
    }
}
