# Game Library

A personal portfolio project for tracking a library of games — a .NET backend API paired with a React frontend, built as two independent apps in one repo.

## Structure

```
Game-Library/                  (repo root)
├── Game-Library-Service/      # Backend — .NET 10 Web API
├── Game-Library-FE/           # Frontend — React + TypeScript (Vite)
└── SeedData/                  # SQL script for local dev sample data (see below)
```

Each is a standalone project with its own dependencies, tooling, and lifecycle — there's no shared build step between them. This root just holds them together and is the actual git repository root.

## Game-Library-Service (backend)

A .NET 10 Web API using Entity Framework Core against SQL Server, a Scalar-powered OpenAPI UI, and a lightweight custom CQRS/Mediator pattern instead of a framework like MediatR.

Current data model: `Game`, `Genre`, and `Publisher` — `Game` has a many-to-one relationship to each of `Genre` and `Publisher`. `GET /api/games`, `GET /api/genres`, and `GET /api/publishers` are all implemented, each paginated and filterable.

Runs either fully in Docker (API + SQL Server via `docker-compose`) or locally against LocalDB / the Dockerized database. Full setup, migration, and troubleshooting instructions are in [`Game-Library-Service/README.md`](Game-Library-Service/README.md) — start there for anything backend-related.

**Quick start:**
```bash
cd Game-Library-Service
docker-compose up -d --build
```
API: `http://localhost:8080` · Scalar UI: `http://localhost:8080/scalar` · Health check: `http://localhost:8080/status`

## Sample data (`SeedData/`)

`SeedData/seeddata.sql` wipes and reseeds `Games`, `Genres`, and `Publishers` with a realistic set of sample data — useful for local development so the frontend has something to show. It is **not** run automatically by migrations or on API startup; you run it yourself, on demand, against a database that already has the schema applied (`dotnet ef database update` first — see the backend README).

The script is written to be run **interactively in a GUI SQL client** (Azure Data Studio / SSMS / DBeaver), not piped through as one unattended batch — see [Connecting to and Querying the Database](Game-Library-Service/README.md#connecting-to-and-querying-the-database) in the backend README for connection details (`localhost,1433`, SQL login `sa` / `YourStrong@Passw0rd`, database `GameLibraryServiceDb`).

1. Open `SeedData/seeddata.sql` in your client, connected to `GameLibraryServiceDb`.
2. Run the top `SELECT * FROM ...` statements if you want to see current contents first.
3. Run the `DELETE FROM` / `DBCC CHECKIDENT ... RESEED, 0` block — this wipes `Games`, `Publishers`, and `Genres` and resets their identity columns back to 0.
4. Run the `BEGIN TRANSACTION` block that inserts the sample `Publishers`, `Genres`, and `Games`.
5. Review the inserted rows, then uncomment and run `COMMIT` at the bottom to keep them (or `ROLLBACK` to discard).

Because it starts a transaction it doesn't commit by default, don't run the whole file as one unattended script (e.g. via `sqlcmd -i`) — the insert will sit uncommitted and hold locks until something commits or rolls it back.

## Game-Library-FE (frontend)

A React 19 + TypeScript app (Vite) with a Games browser and a Publishers browser — both with search, filtering, and pagination against the backend API — plus a system/light/dark theme toggle. See [`Game-Library-FE/README.md`](Game-Library-FE/README.md) for frontend-specific setup.

**Quick start:**
```bash
cd Game-Library-FE
npm install
npm run dev
```

## Status

| Part | State |
|---|---|
| Backend | `Game`, `Genre`, `Publisher` entities with migrations in place; `GET /api/games`, `GET /api/genres`, `GET /api/publishers` implemented and tested. No write endpoints (create/update/delete) yet. |
| Frontend | Games and Publishers browser pages (search/filter/pagination) wired to the backend, plus a theme toggle. |

## Tech Stack

- **Backend**: .NET 10, ASP.NET Core, Entity Framework Core, SQL Server, Scalar (OpenAPI UI), Docker
- **Frontend**: React 19, TypeScript, Vite
