using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ArcherMobilApp.BLL.Contracts;
using ArcherMobilApp.BLL.Models;
using ArcherMobilApp.DAL.MsSql.Contract;
using ArcherMobilApp.DAL.MsSql.Models;
using AutoMapper;

namespace ArcherMobilApp.BLL
{
    public class UsersService : IUsersService
    {
        private readonly IMapper _mapper;

        public IUserRepository _usersRepo { get; }

        public UsersService(IMapper mapper, IUserRepository usersRepo)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _usersRepo = usersRepo ?? throw new ArgumentNullException(nameof(usersRepo));
        }

        public async Task<List<User>> GetUsersAsync()
        {
            var users = await _usersRepo.GetUsersAsync();
            return _mapper.Map<List<User>>(users);
        }

        public async Task<bool> CreateUserAsync(string id, User user)
        {
            return await _usersRepo.CreateUserAsync(id,_mapper.Map<UserEntity>(user));
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            return await _usersRepo.UpdateUserAsync(_mapper.Map<UserEntity>(user));
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var result = await _usersRepo.DeleteUserAsync(id);
            return result;
        }

        public Task<bool> UpdateLastUserLoginAsync(string userId, DateTime lastLogin)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateStatusICRAsync(string userId, bool toReset)
        {
            return await _usersRepo.UpdateStatusICR(userId, toReset);
        }

        public Task<IEnumerable<User>> GetUsersListAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserAsync(string id)
        {
            var user = await _usersRepo.GetUserAsync(id);
            return _mapper.Map<User>(user);
        }

        public async Task<IEnumerable<User>> GetUsersAsync(int pageNumber, int pageSize)
        {
            var users = await _usersRepo.GetUsersAsync(pageNumber, pageSize);
            return _mapper.Map<IEnumerable<User>>(users);
        }
    }
}
