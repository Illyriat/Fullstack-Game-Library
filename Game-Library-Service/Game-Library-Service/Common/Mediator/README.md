# MediatR-like Pattern Implementation Guide

## Overview

This is a lightweight, dependency-free implementation of the CQRS (Command Query Responsibility Segregation) pattern used throughout this service. It separates read operations (queries) from write operations (commands) and dispatches them through a single `IMediator`.

**Key convention**: `Query`/`Command`, `Result`, and `Handler` are all nested inside one outer class named after the operation (e.g. `GetGameById`), rather than living in separate files. This keeps each operation's request, response, and handler together.

## Why Direct DbContext Instead of Repository?

- EF Core's `DbContext` already implements Unit of Work and Repository patterns.
- A repository layer on top adds abstraction without much benefit and can limit access to EF Core features (`Include`, raw SQL, compiled queries, etc.).
- Handlers depend directly on `ApplicationDbContext`, which keeps the code simple and easy to test with an in-memory provider.

## Basic Usage

### Query

```csharp
public class GetGameById
{
    public class Query : IQuery<Result>
    {
        public int GameId { get; init; }
    }

    public class Result
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public int ReleaseYear { get; init; }
    }

    public class Handler : IQueryHandler<Query, Result>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context) => _context = context;

        public async Task<Result> HandleAsync(Query query, CancellationToken token)
        {
            var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == query.GameId, token)
                ?? throw new NotFoundException($"Game with ID {query.GameId} not found");

            return new Result { Id = game.Id, Name = game.Name, ReleaseYear = game.ReleaseYear };
        }
    }
}
```

### Command

```csharp
public class CreateGame
{
    public class Command : ICommand<int>
    {
        public required string Name { get; init; }
        public int ReleaseYear { get; init; }
    }

    public class Handler : ICommandHandler<Command, int>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context) => _context = context;

        public async Task<int> HandleAsync(Command command, CancellationToken token)
        {
            var game = new Game { Name = command.Name, ReleaseYear = command.ReleaseYear };

            _context.Games.Add(game);
            await _context.SaveChangesAsync(token);

            return game.Id;
        }
    }
}
```

### Registering a handler

Add each handler to `ServiceCollectionExtensions.ConfigureMediatorAndHandlers` in `Common/Extensions/Startup`:

```csharp
services.AddScoped<IQueryHandler<GetGameById.Query, GetGameById.Result>, GetGameById.Handler>();
services.AddScoped<ICommandHandler<CreateGame.Command, int>, CreateGame.Handler>();
```

### Calling from a controller

```csharp
[HttpGet("{id}")]
public async Task<ActionResult<GetGameById.Result>> GetGame(int id, CancellationToken cancellationToken)
{
    var result = await _mediator.SendQueryAsync<GetGameById.Query, GetGameById.Result>(new GetGameById.Query { GameId = id }, cancellationToken);
    return Ok(result);
}
```

## Naming conventions

- Outer class: verb + noun, no suffix (`GetGameById`, `CreateGame`, `DeleteGame`).
- Nested classes are always named `Query`/`Command`, `Result`, `Handler`, and (optionally) `Validator`.
- Handlers are registered with `AddScoped` since most depend on `ApplicationDbContext`, which is itself scoped.
