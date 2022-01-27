using Microsoft.EntityFrameworkCore;

using Archer.AMA.DAL.Contract;
using Archer.AMA.DAL.EntityFramework.Base;
using Archer.AMA.Entity;

namespace ArcherMobilApp.DAL.MsSql
{
    public class JobOpportunityRepository : EntityFrameworkRepositoryBase<JobOpportunityEntity, int?>, IJobOpportunityRepository
    {
        public JobOpportunityRepository(DbContextOptions<ArcherContext> options) : base(options)
        {

        }

        protected override bool IsEnitytNew(JobOpportunityEntity entity)
        {
            return !entity.Id.HasValue;
        }
    }
}
