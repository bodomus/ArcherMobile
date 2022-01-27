using Microsoft.EntityFrameworkCore;

namespace Archer.AMA.DAL.EntityFramework.Base
{

    /// <summary>
    /// Generic storage context provides access to the Archer storage.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TID"></typeparam>
    public class ArcherContext<TEntity, TID> : ArcherContext where TEntity : class
    {
        public ArcherContext(DbContextOptions options, QueryTrackingBehavior queryTrackingBehavior)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = queryTrackingBehavior;
        }

        public ArcherContext(DbContextOptions options)
            : this(options, QueryTrackingBehavior.TrackAll)
        {

        }

        /// <summary>
        /// Storage set of enitities.
        /// </summary>
        public DbSet<TEntity> Entities { get; set; }
    }

    /// <summary>
    /// Storage context provides access to the Archer storage.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TID"></typeparam>
    public class ArcherContext : DbContext
    {
        public ArcherContext(DbContextOptions options)
            : this(options, QueryTrackingBehavior.TrackAll)
        {

        }

        public ArcherContext(DbContextOptions options, QueryTrackingBehavior queryTrackingBehavior)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = queryTrackingBehavior;
        }
    }
}
