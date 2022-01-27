using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using ArcherMobilApp.DAL.MsSql.Contract;
using ArcherMobilApp.DAL.MsSql.Models;
using Archer.AMA.Entity;

namespace ArcherMobilApp.DAL.MsSql
{
    public class UsersRepository : BaseRepository, IUserRepository//, IHealthCheck
    {
        public UsersRepository(string connectionString, IRepositoryContextFactory contextFactory) : base(connectionString, contextFactory)
        {
        }

        public RepositoryContext GetContext()
        {
            using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                return context;
            }
        }

        public async Task<ComplexUserEntity> GetUserAsync(string id)
        {
            await using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                return await context.GetUserAsync(id);
            }
        }

        public async Task<bool> CreateUserAsync(string userId, UserEntity complexUser)
        {
            await using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                //var user = new UserEntity() { IsAdmin = complexUser.IsAdmin, ConfirmICR = false, KeepLogged = false, Id = complexUser.Id};

                context.Users.Add(complexUser);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> UpdateUserAsync(UserEntity complexUser)
        {
            await using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var u = context.Users.Find(complexUser.Id);
                if (u == null)
                    throw new InvalidOperationException($"User {complexUser.Id} not found.");
                u.ConfirmICR = complexUser.ConfirmICR;
                u.KeepLogged = complexUser.KeepLogged;
                u.UserName = complexUser.UserName;
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            await using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var u = context.Users.Find(userId);
                if (u == null)
                    throw new InvalidOperationException($"User {userId} not found.");
                context.Users.Remove(u);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> UpdateLastUserLoginAsync(string userId, DateTime lastLogin)
        {
            await using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var u = context.Users.Find(userId);
                if (u == null)
                    throw new InvalidOperationException($"User {userId} not found.");
                u.LastLogin = lastLogin;
                await context.SaveChangesAsync();

                return true;
                //await context.UpdateLastUserLoginAsync(userId, lastLogin);
            }
        }

        public async Task<bool> UpdateStatusICR(string userId, bool toReset)
        {
            await using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var u = context.Users.Find(userId);
                if (u == null)
                    throw new InvalidOperationException($"User {userId} not found.");
                u.ConfirmICR = toReset;
                u.ICRConfirmationDate = DateTime.Now;
                await context.SaveChangesAsync();

                return true;
                //await context.UpdateStatusICRAsync(userId, toReset);
            }
        }

        public async Task<bool> UpdateKeepLoggedFlag(string userId, bool toReset)
        {
            await using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var u = context.Users.Find(userId);
                if (u == null)
                    throw new InvalidOperationException($"User {userId} not found.");
                u.KeepLogged = toReset;
                await context.SaveChangesAsync();

                return true;
                //await context.UpdateStatusICRAsync(userId, toReset);
            }
        }



        public async Task<IEnumerable<DocumentEntity>> GetDocuments()
        {
            await using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                return await context.Documents.ToListAsync();
            }
        }

        public async Task<IEnumerable<AnnouncementEntity>> GetAnnouncements()
        {
            await using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                return await context.Announcements.ToListAsync();
            }
        }

        public async Task<bool> CreateAnnouncement(AnnouncementEntity announcement)
        {
            await using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                context.Announcements.Add(announcement);
                await context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> UpdateAnnouncement(int id, AnnouncementEntity announcement)
        {
            await using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                context.Announcements.Update(announcement);
                await context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> DeleteAnnouncement(int id)
        {
            await using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var an = context.Announcements.SingleOrDefault(a => a.Id == id);
                context.Announcements.Remove(an);
                await context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<IEnumerable<UserEntity>> GetUsersAsync(int pageNumber, int pageSize)
        {
            await using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                return await context.GetUsersAsync(pageNumber, pageSize);
            }
        }

        public async Task<UserEntity> GetUserByEmailAsync(string id)
        {
            await using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                return await context.Users.SingleOrDefaultAsync();
            }
        }

        public async Task<IEnumerable<RoomEntity>> GetRooms()
        {
            await using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                return await context.Rooms.ToListAsync();
            }
        }

        public async Task<bool> AddRoom(RoomEntity room)
        {
            await using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                await context.Rooms.AddAsync(room);
                await context.SaveChangesAsync();
                return true;
            }
        }
        public async Task<bool> DeleteRoom(RoomEntity room)
        {
            await using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                context.Rooms.Remove(room);
                await context.SaveChangesAsync();
                return true;
            }
        }

        //Token 
        public async Task<Tuple<string, string>> GetRefreshToken(string userId)
        {
            await using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var u = context
                    .Users
                    .AsEnumerable()
                    .Where(a=>  string.CompareOrdinal(a.Email, userId) == 0).ToList();
                    
                if (!u.Any() || u.Count >  1)
                    throw new InvalidOperationException($"invalid  user {userId}.");

                return new Tuple<string, string>(u.SingleOrDefault()?.Id, u.SingleOrDefault()?.RefreshToken);
            }
        }

        public async Task<bool> AddRefreshToken(string userId, string refreshToken)
        {
            await using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var u = context.Users.Find(userId);
                if (u == null)
                    throw new InvalidOperationException($"User {userId} not found.");

                u.RefreshToken = refreshToken;
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> RemoveRefreshToken(string userId)
        {
            await using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                var u = context.Users.Find(userId);
                if (u == null)
                    throw new InvalidOperationException($"User {userId} not found.");

                u.RefreshToken = string.Empty;
                await context.SaveChangesAsync();
                return true;
            }
        }

        //Token

        #region Roles
        public async Task<IList<RoleEntity>> GetRolesAsync()
        {
            await using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                 return await context.Roles.ToListAsync();
            }
        }

        public async Task<List<ComplexUserEntity>> GetUsersAsync()
        {
            await using (var context = ContextFactory.CreateDbContext(ConnectionString))
            {
                return await context.GetUsersAsync();
            }
        }

        public async Task<bool> ResetIcrStatus()
        {
            try
            {
                await using (var context = ContextFactory.CreateDbContext(ConnectionString))
                {
                    await context.Users.ForEachAsync(a =>
                    {
                        a.ConfirmICR = false;
                        a.ICRConfirmationDate = null;
                    });
                    context.SaveChanges();
                }
                return true;
            }
            catch (DbUpdateException e) {
                return false;
            }
        }
        #endregion
    }
}
