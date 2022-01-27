using Microsoft.EntityFrameworkCore;

using Archer.AMA.DAL.Contract;
using Archer.AMA.DAL.EntityFramework.Base;
using Archer.AMA.Entity;
using ArcherMobilApp.DAL.MsSql.Models;

namespace ArcherMobilApp.DAL.MsSql
{
    public class AppoinmentRepository : EntityFrameworkRepositoryBase<AppoinmentEntity, int?>, IAppoinmentRepository
    {
        public AppoinmentRepository(DbContextOptions<ArcherContext> options) : base(options)
        {

        }

        protected override bool IsEnitytNew(AppoinmentEntity entity)
        {
            return !entity.Id.HasValue;
        }
    }
}
