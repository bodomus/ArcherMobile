using Archer.AMA.DAL.Contract.Base;
using Archer.AMA.Entity;

namespace Archer.AMA.DAL.Contract
{

    /// <summary>
    /// Provides Data Storage repository for the Documents
    /// </summary>
    public interface IDocumentRepository : IEditableRepository<DocumentEntity, int?>
    {

    }
}
