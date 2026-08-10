namespace Game_Library_Service.Data.Entities
{
    public class Publisher : AuditableEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        /// <summary>
        /// Navigation property to games published by this publisher
        /// </summary>
        public virtual ICollection<Game> Games { get; set; } = new List<Game>();
    }
}
