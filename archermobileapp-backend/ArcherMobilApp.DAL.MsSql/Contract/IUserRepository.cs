using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Archer.AMA.Entity;
using ArcherMobilApp.DAL.MsSql.Models;

namespace ArcherMobilApp.DAL.MsSql.Contract
{
    public interface IUserRepository
    {
        RepositoryContext GetContext();
        Task<bool> CreateUserAsync(string id, UserEntity user);
        Task<ComplexUserEntity> GetUserAsync(string id);

        Task<UserEntity> GetUserByEmailAsync(string id);
        Task<IEnumerable<UserEntity>> GetUsersAsync(int pageNumber, int pageSize);
        Task<List<ComplexUserEntity>> GetUsersAsync();
        Task<bool> UpdateUserAsync(UserEntity user);
        Task<bool> DeleteUserAsync(string id);
        Task<bool> UpdateLastUserLoginAsync(string id, DateTime lastLogin);
        Task<bool> UpdateStatusICR(string userId, bool toReset);
        Task<bool> UpdateKeepLoggedFlag(string userId, bool toReset);
        Task<IEnumerable<DocumentEntity>> GetDocuments();
        //Announcments
        Task<IEnumerable<AnnouncementEntity>> GetAnnouncements();
        Task<bool> CreateAnnouncement(AnnouncementEntity announcement);
        Task<bool> UpdateAnnouncement(int id, AnnouncementEntity announcement);
        Task<bool> DeleteAnnouncement(int id);
        //Announcments
        Task<IEnumerable<RoomEntity>> GetRooms();
        Task<bool> AddRoom(RoomEntity room);
        Task<bool> DeleteRoom(RoomEntity room);

        //Token
        Task<Tuple<string,string>> GetRefreshToken(string userId);
        Task<bool> AddRefreshToken(string userId, string refreshToken);
        Task<bool> RemoveRefreshToken(string userId);
        Task<bool> ResetIcrStatus();
        //Token
        #region Roles

        Task<IList<RoleEntity>> GetRolesAsync(); 
        #endregion
    }
}
