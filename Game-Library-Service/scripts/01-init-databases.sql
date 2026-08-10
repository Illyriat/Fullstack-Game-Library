-- Create databases for Game-Library-Service
-- This script runs automatically the first time the SQL Server container
-- starts against an empty data volume. It will NOT re-run on subsequent
-- starts, and it will NOT run against a volume that already has data.

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'GameLibraryServiceDb')
BEGIN
    CREATE DATABASE [GameLibraryServiceDb];
    PRINT 'Created database: GameLibraryServiceDb';
END
ELSE
BEGIN
    PRINT 'Database GameLibraryServiceDb already exists';
END

ALTER DATABASE [GameLibraryServiceDb] SET RECOVERY SIMPLE;

PRINT 'Database initialization completed successfully';

-- To provision additional databases on this same SQL Server instance
-- (e.g. for another service you're running locally), add more
-- CREATE DATABASE blocks here, or a new numbered script in this folder
-- (e.g. 02-init-other-db.sql). See the "Running another database on the
-- same server" section in README.md for how to apply this without
-- losing existing data.
