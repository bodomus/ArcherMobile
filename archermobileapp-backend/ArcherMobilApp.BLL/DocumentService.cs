using Archer.AMA.BLL.Base;
using Archer.AMA.BLL.Contracts;
using Archer.AMA.DAL.Contract;
using Archer.AMA.DTO;
using Archer.AMA.Entity;
using AutoMapper;

namespace ArcherMobilApp.BLL
{
    public class DocumentService : RepositoryServiceBase<IDocumentRepository, DocumentEntity, DocumentDTO, int?>, IDocumentService
    {
        public DocumentService(IDocumentRepository repository, IMapper mapper) : base(repository, mapper)
        { }
    }
}
