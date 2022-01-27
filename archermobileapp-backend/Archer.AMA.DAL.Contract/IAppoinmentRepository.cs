using Archer.AMA.DAL.Contract.Base;
using Archer.AMA.Entity;
using ArcherMobilApp.DAL.MsSql.Models;

namespace Archer.AMA.DAL.Contract
{
    /// <summary>
    /// Provides Data Storage repository for the JobOpportunity
    /// </summary>
    public interface IAppoinmentRepository : IEditableRepository<AppoinmentEntity, int?>
    {
    }
}
