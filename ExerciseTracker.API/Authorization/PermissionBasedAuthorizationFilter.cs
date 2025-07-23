using ExerciseTracker.Core.Models;
using ExerciseTracker.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace ExerciseTracker.API.Authorization;

public class PermissionBasedAuthorizationFilter(AppDbContext dbContext) : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var attribute = (CheckPermissionAttribute)context.ActionDescriptor.EndpointMetadata.FirstOrDefault(x => x is CheckPermissionAttribute);
        if (attribute != null)
        {
            var claimsIdentity = context.HttpContext.User.Identity as ClaimsIdentity;
            if (claimsIdentity == null || !claimsIdentity.IsAuthenticated)
            {
                context.Result = new ForbidResult();
            }
            else
            {
                var userId = int.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
                var hasPermissions = dbContext.Set<UserPermission>().Any(x => x.UserId == userId &&
                x.PermissionId == attribute.permission);
                if (!hasPermissions)
                {
                    context.Result = new ForbidResult();
                }
            }
        }
    }
}
