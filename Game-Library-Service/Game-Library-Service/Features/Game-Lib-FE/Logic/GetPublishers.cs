using Game_Library_Service.Common.Mediator.Interfaces;
using Game_Library_Service.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Game_Library_Service.Features.Game_Lib_FE.Logic
{
    /// <summary>
    /// Retrieves a paginated, filterable list of publishers.
    /// </summary>
    public class GetPublishers
    {
        public const int PageSize = 50;

        public class Query : IQuery<Result>
        {
            /// <summary>
            /// Filters publishers whose name contains this value (case-insensitive).
            /// </summary>
            public string? Name { get; init; }

            /// <summary>
            /// 1-based page number. Defaults to the first page.
            /// </summary>
            public int Page { get; init; } = 1;
        }

        public class Result
        {
            public List<PublisherSummary> Items { get; init; } = [];
            public int Page { get; init; }
            public int PageSize { get; init; }
            public int TotalCount { get; init; }
            public int TotalPages { get; init; }
        }

        public class PublisherSummary
        {
            public int Id { get; init; }
            public string Name { get; init; } = string.Empty;
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

                var publishers = _context.Publishers.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(query.Name))
                {
                    var name = query.Name.Trim();

                    publishers = _useSqlServerCollation
                        ? publishers.Where(p => EF.Functions.Like(EF.Functions.Collate(p.Name, SearchCollation), $"%{name}%"))
                        : publishers.Where(p => p.Name.ToLower().Contains(name.ToLowerInvariant()));
                }

                var totalCount = await publishers.CountAsync(token);

                var items = await publishers
                    .OrderBy(p => p.Name)
                    .ThenBy(p => p.Id)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize)
                    .Select(p => new PublisherSummary
                    {
                        Id = p.Id,
                        Name = p.Name
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
