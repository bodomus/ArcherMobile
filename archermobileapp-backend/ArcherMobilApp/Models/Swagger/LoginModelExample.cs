using Swashbuckle.AspNetCore.Filters;

namespace ArcherMobilApp.Models.Swagger
{
    public class LoginModelExample : IExamplesProvider<LoginViewModel>
    {
        public LoginViewModel GetExamples()
        {
            return new LoginViewModel
            {
                Email = "test@gmail.com",
                IsEmployee = true,
                Password = "123456",
                RememberMe = true
            };
        }
    }
}