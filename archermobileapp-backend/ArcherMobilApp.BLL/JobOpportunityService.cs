using AutoMapper;

using Archer.AMA.BLL.Base;
using Archer.AMA.DAL.Contract;
using Archer.AMA.DTO;
using Archer.AMA.Entity;
using Archer.AMA.BLL.Contracts;

namespace ArcherMobilApp.BLL
{
    public class JobOpportunityService : RepositoryServiceBase<IJobOpportunityRepository, JobOpportunityEntity, JobOpportunityDTO, int?>, IJobOpportunityService
    {
        public JobOpportunityService(IJobOpportunityRepository repository, IMapper mapper) : base(repository, mapper)
        { }
    }
}
