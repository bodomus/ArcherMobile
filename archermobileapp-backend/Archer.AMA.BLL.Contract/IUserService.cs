using Archer.AMA.BLL.Contract.Base;
using Archer.AMA.DTO;
using System.Threading.Tasks;

namespace Archer.AMA.BLL.Contracts
{
    public interface IUserService : IRepositoryService<UserDTO, string>
    {
        /// <summary>
        /// Returns the current user.
        /// </summary>
        /// <param name="executorIdentity"></param>
        /// <returns></returns>
        Task<UserDTO> GetCurrentAsync(string executorIdentity);

        /// <summary>
        /// Confirms ICR flag for current user.
        /// </summary>
        /// <param name="executorIdentity"></param>
        /// <returns></returns>
        Task ConfirmICR(string executorIdentity);

        /// <summary>
        /// Confirms ICR flag for all users.
        /// </summary>
        /// <param name="executorIdentity"></param>
        /// <returns></returns>
        Task ResetICR(string executorIdentity);

        /// <summary>
        /// Update current user name.
        /// </summary>
        /// <param name="name"> New user name</param>
        /// <param name="executorIdentity"></param>
        /// <returns></returns>
        Task UpdateMobilUser(string userName, string email, string executorIdentity);
    }
}
