using Microsoft.EntityFrameworkCore;

using Archer.AMA.DAL.Contract;
using Archer.AMA.DAL.EntityFramework.Base;
using Archer.AMA.Entity;

namespace ArcherMobilApp.DAL.MsSql
{
    /// <summary>
    /// Provides Data Storage repository for the Documents
    /// </summary>
    public class DocumentsRepository : EntityFrameworkRepositoryBase<DocumentEntity, int?>, IDocumentRepository
    {
        public DocumentsRepository(DbContextOptions<ArcherContext> options) : base(options)
        {
            
        }

        protected override bool IsEnitytNew(DocumentEntity entity)
        {
            return !entity.Id.HasValue;
        }
    }
}
