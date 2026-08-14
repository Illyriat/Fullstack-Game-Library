namespace Game_Library_Service.Data.Entities
{
    public class Genre : AuditableEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        /// <summary>
        /// Navigation property to games in this genre
        /// </summary>
        public virtual ICollection<Game> Games { get; set; } = new List<Game>();
    }
}
