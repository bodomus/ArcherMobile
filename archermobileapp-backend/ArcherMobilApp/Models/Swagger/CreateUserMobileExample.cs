using System;

using Swashbuckle.AspNetCore.Filters;

using ArcherMobilApp.BLL.Models;

namespace ArcherMobilApp.Models.Swagger
{
    public class UserMobileExample : IExamplesProvider<UserMobile>
    {
        public UserMobile GetExamples()
        {
            var dnow = DateTime.UtcNow;
            return new UserMobile()
            {
                Id = "1d5bbf3a-5054-45bf-a0fb-78f67c436bbc",
                UserName = "User",
                NewEmail = "test@test.com",
                OldPassword = "OldPassword",
                NewPassword = "NewPassword",
            };
        }
    }
}
