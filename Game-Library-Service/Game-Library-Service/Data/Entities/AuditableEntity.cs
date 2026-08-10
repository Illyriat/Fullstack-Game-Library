using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Game_Library_Service.Data.Entities
{
    /// <summary>
    /// Base class for entities that require audit tracking
    /// </summary>
    public abstract class AuditableEntity
    {
        /// <summary>
        /// Date and time when the entity was created
        /// </summary>
        [Column(TypeName = "datetime2")]
        public DateTime CreatedDateUtc { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Date and time when the entity was last updated
        /// </summary>
        [Column(TypeName = "datetime2")]
        public DateTime? ModifiedDateUtc { get; set; }

        /// <summary>
        /// User ID who created the entity
        /// </summary>
        [MaxLength(100)]
        public string? CreatedBy { get; set; }

        /// <summary>
        /// User ID who last updated the entity
        /// </summary>
        [MaxLength(100)]
        public string? ModifiedBy { get; set; }

        /// <summary>
        /// Soft delete flag - indicates if the entity has been deleted
        /// </summary>
        public bool Deleted { get; set; } = false;
    }
}
