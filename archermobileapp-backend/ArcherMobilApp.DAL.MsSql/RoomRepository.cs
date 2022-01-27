using Microsoft.EntityFrameworkCore;

using Archer.AMA.DAL.Contract;
using Archer.AMA.DAL.EntityFramework.Base;
using ArcherMobilApp.DAL.MsSql.Models;

namespace ArcherMobilApp.DAL.MsSql
{
    /// <summary>
    /// Provides Data Storage repository for the Documents
    /// </summary>
    public class RoomRepository : EntityFrameworkRepositoryBase<RoomEntity, int?>, IRoomRepository
    {
        public RoomRepository(DbContextOptions<ArcherContext> options) : base(options)
        {

        }

        protected override bool IsEnitytNew(RoomEntity entity)
        {
            return !entity.Id.HasValue;
        }
    }
}
