using Swashbuckle.AspNetCore.Filters;

namespace ArcherMobilApp.Models.Swagger
{
    public class SetFlagRequestModelExample : IExamplesProvider<SimpleRequest>
    {
        public SimpleRequest GetExamples()
        {
            return new SimpleRequest()
            {
                Value = "test@archer-soft.com",
                Key = "true"
            };
        }
    }
}
