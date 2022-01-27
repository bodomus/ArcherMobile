using Archer.AMA.DAL.Contract.Base;
using Archer.AMA.Entity;
using ArcherMobilApp.DAL.MsSql.Models;
using System.Threading.Tasks;

namespace Archer.AMA.DAL.Contract
{
    /// <summary>
    /// Provides Data Storage repository for the Users
    /// </summary>
    public interface IUserRepository : IEditableRepository<UserEntity, string>
    {
        /// <summary>
        /// Returns the current user.
        /// </summary>
        /// <param name="executorIdentity"></param>
        /// <returns></returns>
        Task<UserEntity> GetCurrentAsync(string executorIdentity);

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

        Task UpdateMobilUser(string name, string email, string executorIdentity);
    }
}
