using AutoMapper;

using Archer.AMA.BLL.Base;
using Archer.AMA.DAL.Contract;
using Archer.AMA.DTO;
using Archer.AMA.Entity;
using Archer.AMA.BLL.Contracts;
using ArcherMobilApp.DAL.MsSql.Models;

namespace ArcherMobilApp.BLL
{
    public class AppoinmentService : RepositoryServiceBase<IAppoinmentRepository, AppoinmentEntity, AppoinmentDTO, int?>, IAppoinmentService
    {
        public AppoinmentService(IAppoinmentRepository repository, IMapper mapper) : base(repository, mapper)
        { }
    }
}
