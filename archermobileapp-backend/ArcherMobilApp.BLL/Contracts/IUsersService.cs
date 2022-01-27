using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ArcherMobilApp.BLL.Models;

namespace ArcherMobilApp.BLL.Contracts
{
    public interface IUsersService
    {
        Task<List<User>> GetUsersAsync();
        Task<User> GetUserAsync(string userId);
        Task<bool> CreateUserAsync(string userId, User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> UpdateLastUserLoginAsync(string userId, DateTime lastLogin);
        Task<bool> UpdateStatusICRAsync(string userId, bool toReset);
        Task<IEnumerable<User>> GetUsersListAsync(int pageNumber, int pageSize);
    }
}
