using Microsoft.IdentityModel.Tokens;

namespace Archer.AMA.WebApi.Security.Authorize
{
    public interface IArcherSecurityTokenValidator
    {
        bool ValidateToken(SecurityToken token, string executorIdentity);
    }
}
