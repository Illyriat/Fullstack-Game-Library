using Game_Library_Service.Data.Contexts;
using Game_Library_Service.Data.Entities;

namespace Game_Library_Service.Tests.Data.Builders
{
    public class GenreBuilder
    {
        private string _name = "Default Genre";

        public GenreBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public Genre Build()
        {
            return new Genre
            {
                Name = _name
            };
        }

        public async Task<Genre> BuildAndAddAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
        {
            var genre = Build();
            context.Genres.Add(genre);
            await context.SaveChangesAsync(cancellationToken);
            return genre;
        }
    }
}
