using System.Collections.Generic;
using System.Threading.Tasks;
using ArcherMobilApp.BLL.Models;

namespace ArcherMobilApp.BLL.Contracts
{
    public interface IUsersMockProxyService
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserById(int id);
    }
}
