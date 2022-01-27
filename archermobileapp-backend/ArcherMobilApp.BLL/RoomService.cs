using Archer.AMA.BLL.Base;
using Archer.AMA.BLL.Contracts;
using Archer.AMA.DAL.Contract;
using Archer.AMA.DTO;
using ArcherMobilApp.DAL.MsSql.Models;
using AutoMapper;

namespace ArcherMobilApp.BLL
{
    public class RoomService : RepositoryServiceBase<IRoomRepository, RoomEntity, RoomDTO, int?>, IRoomService
    {
        public RoomService(IRoomRepository repository, IMapper mapper) : base(repository, mapper)
        { }
    }
}
