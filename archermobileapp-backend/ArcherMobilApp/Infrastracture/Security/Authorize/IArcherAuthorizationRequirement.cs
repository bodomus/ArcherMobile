using Microsoft.AspNetCore.Authorization;

namespace Archer.AMA.WebApi.Security.Authorize
{
    public interface IArcherAuthorizationRequirement : IAuthorizationRequirement
    {
        bool Authorize(string userName, string actionKey);
    }
}
