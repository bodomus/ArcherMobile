using Microsoft.AspNetCore.Authorization;

namespace Archer.AMA.WebApi.Security.Authorize
{
    public class ArcherAuthorizeAttibute : AuthorizeAttribute
    {
        public const string PolicyName = "ArcherAuthorize";
        public ArcherAuthorizeAttibute()
        {
            Policy = PolicyName;
        }
    }
}
