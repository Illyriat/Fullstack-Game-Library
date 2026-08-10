using Game_Library_Service.Data.Enums;

namespace Game_Library_Service.Data.Entities
{
    public class Game : AuditableEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int ReleaseYear { get; set; }
        public required Genre Genre { get; set; }

        /// <summary>
        /// Publisher ID (foreign key)
        /// </summary>
        public int? PublisherId { get; set; }

        /// <summary>
        /// Navigation property to the publisher
        /// </summary>
        public virtual Publisher? Publisher { get; set; }
    }
}
