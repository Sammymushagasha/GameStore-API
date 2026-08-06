# GameStore-API

## Run API + Blazor Frontend with Docker

This repository includes Docker support for both apps:

- `GameStore.Api` (API)
- `GameStore-Frontend/BlazorApp1` (Blazor Server frontend)

### Prerequisites

- Docker Desktop installed and running.

### Build and start both services

From the repository root, run:

```powershell
docker compose up --build -d
```

### Service URLs

- Frontend: `http://localhost:5169`
- API: `http://localhost:5086`

### Check status and logs

```powershell
docker compose ps
docker compose logs -f gamestore-api
docker compose logs -f gamestore-frontend
```

### Stop services

```powershell
docker compose down
```

### Database persistence

The API uses SQLite and stores the DB in a named Docker volume (`gamestore-db`).

- `docker compose down` keeps data.
- `docker compose down -v` removes data.