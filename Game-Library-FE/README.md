# Game-Library-FE

A React 19 + TypeScript frontend (Vite) for the Game Library API — a Games browser and a Publishers browser, each with search, filtering, and pagination, plus a system/light/dark theme toggle.

## Prerequisites

- **Node.js** and npm.
- The backend API running somewhere reachable — see [`Game-Library-Service/README.md`](../Game-Library-Service/README.md) for how to start it (defaults to `http://localhost:5221`).

## Quick Start

```bash
npm install
cp .env.example .env   # only needed if .env doesn't already exist
npm run dev
```

Vite serves the app at `http://localhost:5173`.

## Configuration

The API base URL is read from the `VITE_API_BASE_URL` environment variable (see `src/vite-env.d.ts` for the typing). `.env` is gitignored; `.env.example` documents the default:

```
VITE_API_BASE_URL=http://localhost:5221
```

Point this at wherever the backend is actually running (e.g. `http://localhost:8080` if you're running it via `docker-compose` — see the backend README).

## What's here

- **Games** (`/games`) — search by name, filter by release year / genre / publisher, paginated table.
- **Publishers** (`/publishers`) — search by name, paginated table.
- Genre and publisher filter options are fetched live from `GET /api/genres` and `GET /api/publishers`, not hardcoded.
- Theme toggle (Auto / Light / Dark) in the header — "Auto" follows the OS preference; an explicit choice is persisted to `localStorage` and overrides it.

No write operations (create/update/delete) yet — the backend only exposes GET endpoints so far.

## Scripts

```bash
npm run dev       # start the dev server
npm run build     # type-check (tsc -b) and build for production
npm run lint      # eslint
npm run preview   # preview a production build locally
```

## React Compiler

The React Compiler is enabled (`babel-plugin-react-compiler` via `@rolldown/plugin-babel` in `vite.config.ts`). This is also why the `react-hooks` ESLint rules are strict about patterns like calling `setState` synchronously inside an effect — see the hooks in `src/hooks/` for the patterns used to stay compliant (e.g. deriving `loading` instead of resetting it eagerly).
