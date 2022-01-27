using Microsoft.IdentityModel.Tokens;

namespace Archer.AMA.WebApi.Security.Authorize
{
    public class ArcherSecurityTokenValidator : IArcherSecurityTokenValidator
    {
        public ArcherSecurityTokenValidator()
        {
        }

        public bool ValidateToken(SecurityToken token, string executorIdentity)
        {
            //TODO: Extra validation for token. E.g. Validation process in database.
            return true;
        }
    }
}
