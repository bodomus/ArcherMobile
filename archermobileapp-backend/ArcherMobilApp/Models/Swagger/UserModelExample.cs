using Swashbuckle.AspNetCore.Filters;
using System;

using ArcherMobilApp.DAL.MsSql.Models;

namespace ArcherMobilApp.Models.Swagger
{
    public class UserModelExample : IExamplesProvider<UserEntity>
    {
        public UserEntity GetExamples()
        {
            return new UserEntity
            {
                Id = Guid.NewGuid().ToString(),
                ConfirmICR = false,
                UserName = "User name",
                Email = "test@email.com"
            };
        }
    }
}
