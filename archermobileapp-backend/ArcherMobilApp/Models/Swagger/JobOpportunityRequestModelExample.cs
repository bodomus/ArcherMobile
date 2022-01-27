using Swashbuckle.AspNetCore.Filters;

using Archer.AMA.DTO;

namespace ArcherMobilApp.Models.Swagger
{
    public class JobOpportunityRequestModelExample : IExamplesProvider<JobOpportunityDTO>
    {
        public JobOpportunityDTO GetExamples()
        {
            return 
                new JobOpportunityDTO(){
                    Id = 1,
                    Description = "Description JobOpportunity",
                    IsArchive = true,
                    RecruiterContacts = "Test RecruiterContacts",
                    Requirements = "Test Requirements",
                    Responsibilities = "Test Responsibilities",
                    StandOut = "Test StandOut", 
                    Title = "Test Title 1"
            };
        }
    }
}