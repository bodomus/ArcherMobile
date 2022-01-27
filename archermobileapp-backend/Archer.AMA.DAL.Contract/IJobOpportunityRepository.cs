using Archer.AMA.DAL.Contract.Base;
using Archer.AMA.Entity;

namespace Archer.AMA.DAL.Contract
{
    /// <summary>
    /// Provides Data Storage repository for the JobOpportunity
    /// </summary>
    public interface IJobOpportunityRepository: IEditableRepository<JobOpportunityEntity, int?>
    {
    }
}
