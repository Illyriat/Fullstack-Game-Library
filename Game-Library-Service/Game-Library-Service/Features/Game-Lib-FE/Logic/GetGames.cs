using Game_Library_Service.Common.Mediator.Interfaces;
using Game_Library_Service.Data.Contexts;
using Game_Library_Service.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace Game_Library_Service.Features.Game_Lib_FE.Logic
{
    /// <summary>
    /// Retrieves a paginated, filterable list of games.
    /// </summary>
    public class GetGames
    {
        public const int PageSize = 50;

        public class Query : IQuery<Result>
        {
            /// <summary>
            /// Filters games whose name contains this value (case-insensitive).
            /// </summary>
            public string? Name { get; init; }

            /// <summary>
            /// Filters games released in this exact year.
            /// </summary>
            public int? ReleaseYear { get; init; }

            /// <summary>
            /// Filters games by genre.
            /// </summary>
            public Genre? Genre { get; init; }

            /// <summary>
            /// Filters games by publisher.
            /// </summary>
            public int? PublisherId { get; init; }

            /// <summary>
            /// 1-based page number. Defaults to the first page.
            /// </summary>
            public int Page { get; init; } = 1;
        }

        public class Result
        {
            public List<GameSummary> Items { get; init; } = [];
            public int Page { get; init; }
            public int PageSize { get; init; }
            public int TotalCount { get; init; }
            public int TotalPages { get; init; }
        }

        public class GameSummary
        {
            public int Id { get; init; }
            public string Name { get; init; } = string.Empty;
            public int ReleaseYear { get; init; }
            public Genre Genre { get; init; }
            public int? PublisherId { get; init; }
            public string? PublisherName { get; init; }
        }

        public class Handler : IQueryHandler<Query, Result>
        {
            private const string SearchCollation = "Latin1_General_100_CI_AI";

            private readonly ApplicationDbContext _context;
            private readonly bool _useSqlServerCollation;

            public Handler(ApplicationDbContext context)
            {
                _context = context;
                _useSqlServerCollation = context.Database.ProviderName?.Contains("SqlServer", StringComparison.OrdinalIgnoreCase) == true;
            }

            public async Task<Result> HandleAsync(Query query, CancellationToken token)
            {
                var page = query.Page < 1 ? 1 : query.Page;

                var games = _context.Games.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(query.Name))
                {
                    var name = query.Name.Trim();

                    games = _useSqlServerCollation
                        ? games.Where(g => EF.Functions.Like(EF.Functions.Collate(g.Name, SearchCollation), $"%{name}%"))
                        : games.Where(g => g.Name.ToLower().Contains(name.ToLowerInvariant()));
                }

                if (query.ReleaseYear.HasValue)
                {
                    games = games.Where(g => g.ReleaseYear == query.ReleaseYear.Value);
                }

                if (query.Genre.HasValue)
                {
                    games = games.Where(g => g.Genre == query.Genre.Value);
                }

                if (query.PublisherId.HasValue)
                {
                    games = games.Where(g => g.PublisherId == query.PublisherId.Value);
                }

                var totalCount = await games.CountAsync(token);

                var items = await games
                    .OrderBy(g => g.Name)
                    .ThenBy(g => g.Id)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize)
                    .Select(g => new GameSummary
                    {
                        Id = g.Id,
                        Name = g.Name,
                        ReleaseYear = g.ReleaseYear,
                        Genre = g.Genre,
                        PublisherId = g.PublisherId,
                        PublisherName = g.Publisher != null ? g.Publisher.Name : null
                    })
                    .ToListAsync(token);

                return new Result
                {
                    Items = items,
                    Page = page,
                    PageSize = PageSize,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize)
                };
            }
        }
    }
}
