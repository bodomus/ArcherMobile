using Swashbuckle.AspNetCore.Filters;

using System.Collections.Generic;
using Archer.AMA.DTO;

namespace ArcherMobilApp.Models.Swagger
{
    public class JobOpportunitiesModelExample : IExamplesProvider<IEnumerable<JobOpportunityDTO>>
    {
        public IEnumerable<JobOpportunityDTO> GetExamples()
        {
            return new List<JobOpportunityDTO>()
            {
                new JobOpportunityDTO(){
                    Id = 1,
                    Description = "Description JobOpportunity",
                    IsArchive = true,
                    RecruiterContacts = "Test RecruiterContacts",
                    Requirements = "Test Requirements",
                    Responsibilities = "Test Responsibilities",
                    StandOut = "Test StandOut", 
                    Title = "Test Title 1"

                },
                new JobOpportunityDTO(){
                    Id = 2,
                    Description = "Description JobOpportunity",
                    IsArchive = true,
                    RecruiterContacts = "Test RecruiterContacts",
                    Requirements = "Test Requirements",
                    Responsibilities = "Test Responsibilities",
                    StandOut = "Test StandOut",
                    Title = "Test Title 2"
                }
            };
        }
    }
}