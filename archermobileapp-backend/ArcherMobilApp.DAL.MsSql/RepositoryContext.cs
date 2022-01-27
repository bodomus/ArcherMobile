using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using ArcherMobilApp.DAL.MsSql.Models;
using Archer.AMA.Entity;

namespace ArcherMobilApp.DAL.MsSql
{
    public class RepositoryContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<AspNetUsersEntity> AspNetUsersEntity { get; set; }
        public DbSet<DocumentEntity> Documents { get; set; }
        public DbSet<AnnouncementEntity> Announcements { get; set; }
        public DbSet<RoomEntity> Rooms { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ComplexUserEntity>().Property(c => c.IsAdmin).HasConversion(v => Convert.ToInt32(v), v => Convert.ToBoolean(v));
            modelBuilder.Entity<ComplexUserEntity>().Property(c => c.LockoutEnabled).HasConversion(v => Convert.ToInt32(v), v => Convert.ToBoolean(v));
            modelBuilder.Entity<ComplexUserEntity>().Property(c => c.ConfirmICR).HasConversion(v => Convert.ToInt32(v), v => Convert.ToBoolean(v));

            modelBuilder.Entity<UserEntity>().Property(f => f.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<UserEntity>().Property(f => f.DateInserted).ValueGeneratedOnAdd();
            modelBuilder.Entity<UserEntity>().Property(f => f.DateDeleted).ValueGeneratedOnAdd();
            modelBuilder.Entity<UserEntity>().Property(f => f.DateUpdated).ValueGeneratedOnAdd();
            modelBuilder.Entity<UserEntity>().Property(f => f.LastLogin).ValueGeneratedOnAdd();

            modelBuilder.Entity<AnnouncementEntity>().Property(f => f.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<AnnouncementEntity>().Property(f => f.DateDeleted).ValueGeneratedOnAdd();
            modelBuilder.Entity<AnnouncementEntity>().Property(f => f.DateUpdated).ValueGeneratedOnAdd();
            modelBuilder.Entity<AnnouncementEntity>().Property(f => f.DateInserted).ValueGeneratedOnAdd();

            modelBuilder.Entity<RoomEntity>().Property(f => f.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<RoomEntity>().Property(f => f.DateDeleted).ValueGeneratedOnAdd();
            modelBuilder.Entity<RoomEntity>().Property(f => f.DateUpdated).ValueGeneratedOnAdd();
            modelBuilder.Entity<RoomEntity>().Property(f => f.DateInserted).ValueGeneratedOnAdd();

            modelBuilder.Entity<RoleEntity>().Property(f => f.Id).ValueGeneratedOnAdd();
        }
        public async Task<ComplexUserEntity> GetUserAsync(string id)
        {
            SqlParameter value1Input = new SqlParameter("userId", id ?? (object)DBNull.Value);
            var result = await Set<ComplexUserEntity>().FromSqlRaw("EXEC sp_GetUsers @userId ", value1Input).ToListAsync();
            return result.SingleOrDefault(a => string.CompareOrdinal(a.UserId, "1") != 0);
        }

        public async Task<List<ComplexUserEntity>> GetUsersAsync()
        {
            SqlParameter value1Input = new SqlParameter("userId", (object)DBNull.Value);
            var result = await Set<ComplexUserEntity>().FromSqlRaw("EXEC sp_GetUsers @userId ", value1Input).ToListAsync();
            var data = result.Where(a => string.CompareOrdinal(a.UserId, "1") != 0).ToList();
            return data;
        }

        public async Task<IEnumerable<UserEntity>> GetUsersAsync(int pageNumber, int pageSize)
        {
            SqlParameter value1Input = new SqlParameter("userId", (object)DBNull.Value);
            var result = await Set<UserEntity>().FromSqlRaw("EXEC sp_GetUsers @userId ", value1Input).ToListAsync();
            var list = result.Where(a => string.CompareOrdinal(a.Id, "1") != 0).Page(pageNumber, pageSize);
            return list;
        }

        public async Task<bool> UpdateUserAsync(UserEntity user)
        {
            var u = Set<UserEntity>().SingleOrDefault(a => string.CompareOrdinal(a.Id, user.Id) == 0);
            u.IsAdmin = user.IsAdmin;
            await this.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            SqlParameter value1Input = new SqlParameter("userId", userId ?? (object)DBNull.Value);
            var result = await Set<UserEntity>().FromSqlRaw("EXEC sp_DeleteUser @userId", value1Input).SingleOrDefaultAsync();
            return true;
        }

        public async Task<bool> UpdateLastUserLoginAsync(string userId, DateTime lastLogin)
        {
            var users = await Set<UserEntity>().ToListAsync();
            
            users.First().LastLogin = lastLogin;
            await this.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateStatusICRAsync(string userId, bool toReset)
        {
            var u = Set<UserEntity>().SingleOrDefault(a => string.CompareOrdinal(a.Id, userId) == 0);
            u.ConfirmICR = toReset;
            await this.SaveChangesAsync();
            return true;
        }
    }
}
