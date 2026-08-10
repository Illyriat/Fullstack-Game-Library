using Game_Library_Service.Data.Contexts;
using Game_Library_Service.Data.Entities;
using Game_Library_Service.Data.Enums;

namespace Game_Library_Service.Tests.Data.Builders
{
    public class GameBuilder
    {
        private string _name = "Default Game";
        private int _releaseYear = 2000;
        private Genre _genre = Genre.Action;
        private Publisher? _publisher;

        public GameBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public GameBuilder WithReleaseYear(int releaseYear)
        {
            _releaseYear = releaseYear;
            return this;
        }

        public GameBuilder WithGenre(Genre genre)
        {
            _genre = genre;
            return this;
        }

        public GameBuilder WithPublisher(Publisher? publisher)
        {
            _publisher = publisher;
            return this;
        }

        public Game Build()
        {
            return new Game
            {
                Name = _name,
                ReleaseYear = _releaseYear,
                Genre = _genre,
                Publisher = _publisher
            };
        }

        public async Task<Game> BuildAndAddAsync(ApplicationDbContext context, CancellationToken cancellationToken = default)
        {
            var game = Build();
            context.Games.Add(game);
            await context.SaveChangesAsync(cancellationToken);
            return game;
        }
    }
}
