using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using ArcherMobilApp.BLL.Models;

namespace ArcherMobilApp.Models.Swagger
{
    public class UsersModelExample : IExamplesProvider<IEnumerable<User>>
    {
        public IEnumerable<User> GetExamples()
        {
            
            return new List<User>() 
            {new User(){
                Id = Guid.NewGuid().ToString(),
                ConfirmICR = false

                },
                new User(){
                    Id = Guid.NewGuid().ToString(),
                    ConfirmICR = true
                }
            };
        }
    }
}
