# Game Library

A personal portfolio project for tracking a library of games — a .NET backend API paired with a React frontend, built as two independent apps in one repo.

## Structure

```
Game-Library/                  (repo root)
├── Game-Library-Service/      # Backend — .NET 10 Web API
└── Game-Library-FE/           # Frontend — React + TypeScript (Vite)
```

Each is a standalone project with its own dependencies, tooling, and lifecycle — there's no shared build step between them. This root just holds them together and is the actual git repository root.

## Game-Library-Service (backend)

A .NET 10 Web API using Entity Framework Core against SQL Server, a Scalar-powered OpenAPI UI, and a lightweight custom CQRS/Mediator pattern instead of a framework like MediatR.

Current data model: `Game` (with a `Genre` enum) and `Publisher`, related one-to-many.

Runs either fully in Docker (API + SQL Server via `docker-compose`) or locally against LocalDB / the Dockerized database. Full setup, migration, and troubleshooting instructions are in [`Game-Library-Service/README.md`](Game-Library-Service/README.md) — start there for anything backend-related.

**Quick start:**
```bash
cd Game-Library-Service
docker-compose up -d --build
```
API: `http://localhost:8080` · Scalar UI: `http://localhost:8080/scalar` · Health check: `http://localhost:8080/status`

## Game-Library-FE (frontend)

A React 19 + TypeScript app scaffolded with Vite. Currently just the default Vite template — no game-library-specific UI has been built yet.

**Quick start:**
```bash
cd Game-Library-FE
npm install
npm run dev
```

## Status

| Part | State |
|---|---|
| Backend | Scaffolded and working — EF Core, Docker, Scalar, CQRS pattern, `Game`/`Publisher` entities and migrations in place. No API endpoints (controllers) built yet. |
| Frontend | Freshly scaffolded, unmodified Vite starter. No game-library UI built yet. |

## Tech Stack

- **Backend**: .NET 10, ASP.NET Core, Entity Framework Core, SQL Server, Scalar (OpenAPI UI), Docker
- **Frontend**: React 19, TypeScript, Vite
