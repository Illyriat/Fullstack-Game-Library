# Game-Library-Service

A .NET 10 Web API for managing a game library, using Entity Framework Core (SQL Server), a Scalar-powered OpenAPI UI, and a lightweight CQRS/Mediator pattern.

## Prerequisites

- **Docker Desktop** — installed and **running**. Docker Compose talks to the Docker Desktop engine, so if Docker Desktop isn't open, every `docker` / `docker-compose` command below will fail with a connection error. Open the app and wait until it shows "Engine running" before continuing.
- **.NET 10 SDK** — for building, running migrations, and running the API outside of Docker.
- (Optional) A SQL client for browsing the database directly — [Azure Data Studio](https://azure.microsoft.com/en-us/products/data-studio), SSMS, or [DBeaver](https://dbeaver.io/) all work fine.

## Quick Start

From the repo root (this file's directory):

```bash
docker-compose up -d --build
```

This starts two containers on a shared Docker network:

| Container | What it is | Port |
|---|---|---|
| `game-library-service-db` | SQL Server 2022 | `1433` |
| `game-library-service-api` | This API (built from the root `Dockerfile`) | `8080` |

On first start (empty volume only), `scripts/01-init-databases.sql` runs automatically and creates the `GameLibraryServiceDb` database.

Check everything came up:

```bash
docker-compose ps
docker-compose logs sqlserver
docker-compose logs game-library-service
```

## Applying Migrations

Migrations run from your host machine against the containerized database — not inside the container. Make sure `docker-compose up` is running first.

```bash
cd Game-Library-Service

# Install the EF Core CLI tool once, if you don't have it
dotnet tool install --global dotnet-ef

# Apply existing migrations to create/update the schema
dotnet ef database update
```

This connects using the connection string in `Game-Library-Service/appsettings.json` / `appsettings.Development.json`, unless overridden by an environment variable (see below).

To add a new migration after changing an entity:

```bash
cd Game-Library-Service
dotnet ef migrations add <DescriptiveName> -o Migrations
dotnet ef database update
```

## Seeding Sample Data

Migrations only create the schema — no rows. For local dev data, `../SeedData/seeddata.sql` (repo root, sibling to this folder) wipes and reseeds `Publishers`, `Genres`, and `Games` with a realistic sample set.

It's written to be run **interactively in a GUI SQL client** (Azure Data Studio / SSMS / DBeaver) connected to `GameLibraryServiceDb` — see [Connecting to and Querying the Database](#connecting-to-and-querying-the-database) below for connection details. Run it in order: the `SELECT` checks (optional), then the `DELETE FROM` / `DBCC CHECKIDENT ... RESEED, 0` block, then the `BEGIN TRANSACTION` insert block, then review and uncomment `COMMIT` once you're happy with what it inserted. Don't run the whole file as one unattended `sqlcmd -i` batch — the insert is deliberately left uncommitted until you say otherwise, and an unattended run will leave it open, holding locks.

## Running the API

**Option A — Fully in Docker (recommended for a quick check):**

Already running after `docker-compose up -d --build`. The API is available at `http://localhost:8080`, with Scalar's UI at `http://localhost:8080/scalar` and a health check at `http://localhost:8080/status`.

**Option B — Run the API on your host, database still in Docker:**

```bash
cd Game-Library-Service
dotnet run
```

This uses `appsettings.Development.json`, which by default points at LocalDB (`(localdb)\mssqllocaldb`), **not** the Dockerized SQL Server. To point your locally-run API at the Dockerized database instead, override the connection string for that run:

```bash
cd Game-Library-Service
dotnet run --ConnectionStrings:DefaultConnection="Server=localhost,1433;Database=GameLibraryServiceDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=true;MultipleActiveResultSets=true"
```

(Windows without Docker at all also works fine — LocalDB is installed with Visual Studio and `dotnet run` will use it directly via `appsettings.Development.json`, no containers required.)

## Connecting to and Querying the Database

**From the command line, via the running container:**

```bash
docker exec -it game-library-service-db /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "YourStrong@Passw0rd" -C
```

Then, at the `1>` prompt:

```sql
USE GameLibraryServiceDb;
GO
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES;
GO
SELECT * FROM Games;
GO
```

Or run a one-off query without an interactive session:

```bash
docker exec -it game-library-service-db /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "YourStrong@Passw0rd" -C -Q "SELECT name FROM sys.databases;"
```

**From a GUI client (Azure Data Studio / SSMS / DBeaver):**

- Server: `localhost,1433`
- Authentication: SQL Login
- Username: `sa`
- Password: `YourStrong@Passw0rd`
- Trust server certificate: yes (self-signed cert in the container)

This works whether the API itself is running in Docker or on your host — SQL Server's port is published to `localhost:1433` either way.

## Running Another Database on the Same Server

The SQL Server container is just one server instance — it can happily host multiple databases, the same way a local SQL Server install would. You don't need a second container.

**Ad-hoc (quickest, doesn't survive `docker-compose down -v`):**

```bash
docker exec -it game-library-service-db /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "YourStrong@Passw0rd" -C -Q "CREATE DATABASE [SomeOtherDb];"
```

**Permanent, via an init script:** add a new numbered file to `scripts/`, e.g. `scripts/02-init-other-db.sql`, following the same `IF NOT EXISTS ... CREATE DATABASE` pattern as `scripts/01-init-databases.sql`. Init scripts only run automatically when the container starts against an **empty** volume, so to apply a new script to an already-provisioned server you either:

- run it directly against the running container (`sqlcmd -i scripts/02-init-other-db.sql`, or paste it into a GUI client), or
- wipe and recreate the volume with `docker-compose down -v && docker-compose up -d --build` (this re-runs every script in `scripts/`, but also **deletes all existing data** in every database on the server — only do this if that's fine).

Point whatever other service/app owns that database at `Server=localhost,1433;Database=SomeOtherDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=true` from your host, or `Server=game-library-service-db;...` from another container on the `game-library-network` Docker network.

## Docker Commands Reference

```bash
# Start (build if needed) in the background
docker-compose up -d --build

# Start with logs attached
docker-compose up

# Stop containers, keep data
docker-compose down

# Stop containers and DELETE all database data
docker-compose down -v

# Follow logs
docker-compose logs -f sqlserver
docker-compose logs -f game-library-service

# Restart one service
docker-compose restart game-library-service
```

## Destroying the Local Containers to Reclaim Memory/Disk

Stopped containers still hold onto their images, and SQL Server's data volume can grow fairly large over time. There are a few levels of cleanup depending on how much you want to reclaim:

**Level 1 — stop the containers, keep everything else (fastest to resume):**

```bash
docker-compose down
```

Containers are removed; the image and the `sqlserver_data` volume (your database) are left alone. Next `docker-compose up -d` picks up right where you left off.

**Level 2 — also delete the database data:**

```bash
docker-compose down -v
```

Removes the containers **and** the `sqlserver_data` volume. This deletes every database on that SQL Server instance (`GameLibraryServiceDb` and anything else you added per the section above). Next `docker-compose up -d --build` starts from a completely empty server and re-runs `scripts/01-init-databases.sql`.

**Level 3 — also delete the built image (reclaims the most disk):**

```bash
docker-compose down -v --rmi all
```

`--rmi all` additionally removes the image(s) built/used by this compose file, including the downloaded `mcr.microsoft.com/mssql/server:2022-latest` base image (a few hundred MB). Next `docker-compose up -d --build` re-downloads and rebuilds from scratch, so this is the slowest to resume from.

**Level 4 — clean up leftover Docker build cache and dangling layers system-wide:**

```bash
docker system prune -f
```

This isn't specific to this project — it clears unused images/containers/networks/build cache across all of Docker on your machine. Add `--volumes` (`docker system prune -f --volumes`) to also remove any unused volumes from *other* projects, but be careful — that's not scoped to Game-Library-Service.

**On Windows, none of the above frees RAM back to Windows itself.** Docker Desktop runs everything inside a WSL2 VM (`vmmem` process in Task Manager), and that VM's memory allocation doesn't shrink just because you removed containers/images inside it — it stays reserved until the VM itself is restarted. If Task Manager still shows `vmmem` holding several GB after cleanup:

```powershell
wsl --shutdown
```

This stops the WSL2 VM entirely, releasing its memory back to Windows. Docker Desktop restarts it automatically next time you launch Docker Desktop or run a `docker` command — you don't need to do anything else to bring it back. If it becomes a recurring problem, Docker Desktop's Settings → Resources lets you cap how much RAM/CPU the VM is allowed to claim in the first place.

## Troubleshooting

**`docker-compose` commands fail immediately / "cannot connect to the Docker daemon"**
Docker Desktop isn't running. Open it and wait for the engine to report ready, then retry.

**Port 1433 already in use**
Something else (a local SQL Server install, another container) is bound to it.
```bash
netstat -an | findstr :1433
```
Stop the conflicting process, or change the host-side port mapping in `docker-compose.yml` (e.g. `"14330:1433"`) and update connection strings accordingly.

**API container exits or logs connection errors on startup**
SQL Server can take several seconds to accept connections after the container starts. Wait ~10–15 seconds and restart the API container:
```bash
docker-compose restart game-library-service
```

**Verify the DB container is actually healthy**
```bash
docker-compose ps
docker exec -it game-library-service-db /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "YourStrong@Passw0rd" -C -Q "SELECT 1"
```

**Using Git Bash on Windows and `sqlcmd` paths get mangled into a `C:\...` path**
Git Bash rewrites leading `/opt/...`-style arguments into Windows paths before they reach `docker exec`. Prefix the command with `MSYS_NO_PATHCONV=1`, e.g. `MSYS_NO_PATHCONV=1 docker exec -it game-library-service-db /opt/mssql-tools18/bin/sqlcmd ...`. PowerShell and cmd.exe don't have this problem.

**Start completely fresh**
```bash
docker-compose down -v
docker-compose up -d --build
cd Game-Library-Service
dotnet ef database update
```

## File Structure

```
Game-Library-Service/                      (repo root)
├── docker-compose.yml                     # Database + API, for local Docker runs
├── Dockerfile                             # Builds/runs the .NET API (build context: repo root)
├── scripts/
│   └── 01-init-databases.sql              # Creates GameLibraryServiceDb on first container start
├── Game-Library-Service/                  # The API project
│   ├── Program.cs
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── Migrations/
│   └── ...
├── Game-Library-Service.Tests/            # xUnit test project
└── README.md                              # This file
```

This folder sits inside the monorepo (`Game-Library/`); `../SeedData/seeddata.sql` (sibling to this folder, see [Seeding Sample Data](#seeding-sample-data) above) is where local dev sample data lives.

## Security Note

`YourStrong@Passw0rd` is a local-development-only default, committed in plain text here on purpose for convenience. Never reuse it, and never carry this pattern into a real deployment — use environment-injected secrets or a managed identity instead.
