using Game_Library_Service.Common.Exceptions.CustomExceptions;
using Game_Library_Service.Common.Mediator.Interfaces;
using Game_Library_Service.Data.Contexts;
using Game_Library_Service.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Game_Library_Service.Features.Game_Lib_FE.Logic
{
    /// <summary>
    /// Creates a new publisher.
    /// </summary>
    public class CreatePublisher
    {
        public const int MaxNameLength = 200;

        public class Command : ICommand<Result>
        {
            /// <summary>
            /// Name of the publisher to create.
            /// </summary>
            public string Name { get; init; } = string.Empty;
        }

        public class Result
        {
            public int Id { get; init; }
            public string Name { get; init; } = string.Empty;
        }

        public class Handler : ICommandHandler<Command, Result>
        {
            private const string SearchCollation = "Latin1_General_100_CI_AI";

            private readonly ApplicationDbContext _context;
            private readonly bool _useSqlServerCollation;

            public Handler(ApplicationDbContext context)
            {
                _context = context;
                _useSqlServerCollation = context.Database.ProviderName?.Contains("SqlServer", StringComparison.OrdinalIgnoreCase) == true;
            }

            public async Task<Result> HandleAsync(Command command, CancellationToken token)
            {
                var name = command.Name?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new BadRequestException("Publisher name is required.");
                }

                if (name.Length > MaxNameLength)
                {
                    throw new BadRequestException($"Publisher name must not exceed {MaxNameLength} characters.");
                }

                // The unique index on Publisher.Name spans soft-deleted rows too, so the duplicate
                // check must ignore the global soft-delete filter to match that DB-level constraint.
                var existingPublishers = _context.Publishers.AsNoTracking().IgnoreQueryFilters();

                var duplicateExists = _useSqlServerCollation
                    ? await existingPublishers.AnyAsync(p => EF.Functions.Collate(p.Name, SearchCollation) == name, token)
                    : await existingPublishers.AnyAsync(p => p.Name.ToLower() == name.ToLowerInvariant(), token);

                if (duplicateExists)
                {
                    throw new ConflictException($"A publisher named '{name}' already exists.");
                }

                var publisher = new Publisher
                {
                    Name = name
                };

                _context.Publishers.Add(publisher);

                try
                {
                    await _context.SaveChangesAsync(token);
                }
                catch (DbUpdateException)
                {
                    throw new ConflictException($"A publisher named '{name}' already exists.");
                }

                return new Result
                {
                    Id = publisher.Id,
                    Name = publisher.Name
                };
            }
        }
    }
}
