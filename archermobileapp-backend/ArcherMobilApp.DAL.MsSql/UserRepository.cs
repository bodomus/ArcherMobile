using Microsoft.EntityFrameworkCore;

using Archer.AMA.DAL.Contract;
using Archer.AMA.DAL.EntityFramework.Base;
using ArcherMobilApp.DAL.MsSql.Models;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace ArcherMobilApp.DAL.MsSql
{
    /// <summary>
    /// Provides Data Storage repository for the Documents
    /// </summary>
    public class UserRepository : EntityFrameworkRepositoryBase<UserEntity, string>, IUserRepository
    {
        public UserRepository(DbContextOptions<ArcherContext> options) : base(options)
        {

        }

        protected override bool IsEnitytNew(UserEntity entity)
        {
            using (var contex = CreateContext())
            {
                return !contex.Entities.Any(obj => obj.Id == entity.Id);
            }
        }

        internal override Task<UserEntity> SaveAsync(ArcherContext<UserEntity, string> context, UserEntity entity, string executorIdentity)
        {

            if (!IsEnitytNew(entity))
            {

                var result = context.Entities.FirstOrDefault(obj => obj.Id == entity.Id);

                context.Entry(result).State = EntityState.Detached;

                entity.ConfirmICR = result.ConfirmICR;
                entity.LastLogin = result.LastLogin;
                entity.KeepLogged = result.KeepLogged;
                entity.RefreshToken = result.RefreshToken;
                entity.DateUpdated = DateTime.UtcNow;
            }

            return base.SaveAsync(context, entity, executorIdentity);
        }

        public async Task<UserEntity> GetCurrentAsync(string executorIdentity)
        {
            using (var context = CreateContext())
            {
                return await context.Entities.SingleAsync(obj => obj.Email == executorIdentity);
            }
        }

        public async Task ConfirmICR(string executorIdentity)
        {
            using (var context = CreateContext())
            {
                var entity = await context.Entities.SingleOrDefaultAsync(obj => obj.Email == executorIdentity);

                if (entity != null)
                {
                    entity.ConfirmICR = true;
                    entity.ICRConfirmationDate = DateTime.UtcNow;
                    context.Update(entity);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task ResetICR(string executorIdentity)
        {
            using (var context = CreateContext())
            {
                var items = await context.Entities.ToListAsync();
                items.ForEach(obj => { 
                    obj.ConfirmICR = false; 
                    obj.ICRConfirmationDate = null; 
                });
                context.UpdateRange(items);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateMobilUser(string userName, string email, string executorIdentity)
        {
            using (var context = CreateContext())
            {
                var entity = await context.Entities.SingleOrDefaultAsync(obj => obj.Email == executorIdentity);

                if (entity != null)
                {
                    entity.UserName = userName;
                    entity.Email = email;
                    entity.DateUpdated = DateTime.Now;
                    context.Update(entity);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
