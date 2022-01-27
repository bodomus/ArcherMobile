using AutoFixture;

using ArcherMobilApp.BLL.Models;

namespace ArcherMobilApp.BLL.Tests.Helpers
{
    public static class Fixtures
    {
        public static User UserFixture(string userName = null)
        {
            var fixture = new Fixture();

            var user = fixture.Build<User>();

            if (!string.IsNullOrEmpty(userName))
            {
                user.With(s => s.UserName, userName);
            }

            return user.Create();
        }
    }
}
