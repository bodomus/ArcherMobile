using Swashbuckle.AspNetCore.Filters;

namespace ArcherMobilApp.Models.Swagger
{
    public class RefreshRequestModelExample : IExamplesProvider<RefreshRequest>
    {
        public RefreshRequest GetExamples()
        {
            return new RefreshRequest
            {
                AccessToken = @"da;lskdl;_))9-asd;l';'sdf",
                RefreshToken = @"ld;'s''f;dlas';lf"
            };
        }
    }
}