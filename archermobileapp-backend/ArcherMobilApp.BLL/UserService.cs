using Archer.AMA.BLL.Base;
using Archer.AMA.BLL.Contracts;
using Archer.AMA.DAL.Contract;
using Archer.AMA.DTO;
using ArcherMobilApp.DAL.MsSql.Models;
using AutoMapper;
using System.Threading.Tasks;

namespace ArcherMobilApp.BLL
{
    public class UserService : RepositoryServiceBase<IUserRepository, UserEntity, UserDTO, string>, IUserService
    {
        public UserService(IUserRepository repository, IMapper mapper) : base(repository, mapper)
        { }

        public async Task ConfirmICR(string executorIdentity)
        {
            await Repository.ConfirmICR(executorIdentity);
        }

        public async Task<UserDTO> GetCurrentAsync(string executorIdentity)
        {
            return ToDTO(await Repository.GetCurrentAsync(executorIdentity));
        }

        public async Task ResetICR(string executorIdentity)
        {
            await Repository.ResetICR(executorIdentity);
        }

        public async Task UpdateMobilUser(string userName, string email, string executorIdentity)
        {
            await Repository.UpdateMobilUser(userName, email, executorIdentity);
        }
    }
}
