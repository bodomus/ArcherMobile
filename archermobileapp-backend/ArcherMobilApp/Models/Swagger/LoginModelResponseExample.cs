using Swashbuckle.AspNetCore.Filters;

namespace ArcherMobilApp.Models.Swagger
{
    public class LoginModelResponseExample : IExamplesProvider<LoginResponse>
    {
        public LoginResponse GetExamples()
        {
            return 
                new LoginResponse(){
                   Id = "1d5bbf3a-5054-45bf-a0fb-78f67c436bbc",
                   AccessToken = "asd';l;l;'s9809804234"
            };
        }
    }
}
