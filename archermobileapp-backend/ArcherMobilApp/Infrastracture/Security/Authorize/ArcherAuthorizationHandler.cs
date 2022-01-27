using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Archer.AMA.WebApi.Security.Authorize
{
    public class ArcherAuthorizationHandler : AuthorizationHandler<IArcherAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IArcherAuthorizationRequirement requirement)
        {
            if (context.Resource is AuthorizationFilterContext resource && context.User.Identity.IsAuthenticated)
            {
                if (requirement.Authorize(context.User.Identity.Name, $"{resource.ActionDescriptor.RouteValues["controller"]}.{resource.ActionDescriptor.RouteValues["action"]}".ToLowerInvariant()))
                    context.Succeed(requirement);
                else
                    context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
