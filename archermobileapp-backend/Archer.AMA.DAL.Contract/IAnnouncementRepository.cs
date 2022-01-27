using Archer.AMA.DAL.Contract.Base;
using ArcherMobilApp.DAL.MsSql.Models;

namespace Archer.AMA.DAL.Contract
{

    /// <summary>
    /// Provides Data Storage repository for the Documents
    /// </summary>
    public interface IAnnouncementRepository : IEditableRepository<AnnouncementEntity, int?>
    {

    }
}
