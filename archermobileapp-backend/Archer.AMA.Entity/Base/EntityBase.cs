using System.ComponentModel.DataAnnotations;

namespace Archer.AMA.Entity.Base
{
#pragma warning disable CS0660
    /// <summary>
    /// Provides base entity functionality
    /// </summary>
    /// <typeparam name="T">Inherit entity type</typeparam>
    /// <typeparam name="ID">ID type</typeparam>
    public abstract class EntityBase<TID>
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        [Key]
        [Required]
        public virtual TID Id { get; set; }
    }
#pragma warning restore CS0660
}
