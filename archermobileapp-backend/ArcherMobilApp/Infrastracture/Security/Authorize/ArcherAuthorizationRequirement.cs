namespace Archer.AMA.WebApi.Security.Authorize
{
    public class ArcherAuthorizationRequirement : IArcherAuthorizationRequirement
    {

        public ArcherAuthorizationRequirement()
        {
        }

        public bool Authorize(string userName, string actionKey)
        {
            //TODO: Add Authorization
            return true;
        }
    }
}
