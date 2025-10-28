# Society Maintenance - Ready-to-deploy project

This archive contains a ready-to-deploy ASP.NET Core 7 Web API backend (SQLite by default) and a simple PWA frontend.
The project is configured for easy local testing with Docker Compose and for container deployment (e.g. Fly.io).

## Quick local run (no Docker)
- Install .NET 7 SDK
- Open `backend/SocietyApi` and run:
  - `dotnet restore`
  - `dotnet ef migrations add InitialCreate` (install dotnet-ef if needed)
  - `dotnet ef database update`
  - `dotnet run`
- Serve the `frontend` folder with any static server (or open index.html directly). Use `apiBase` in the browser console to point to your API base if different origin.

## Docker Compose (simple)
- `docker compose up --build`
- API available at http://localhost:8080
- Frontend available at http://localhost:8081

## Deploying to production
- Database: You can use Supabase (free tier) or any managed Postgres. Set the connection string in environment variable `ConnectionStrings__DefaultConnection`.
- Hosting: Containerize and deploy the API to Fly.io / Render / Railway. For Fly, set the env vars with supabase postgres string and JWT key.
- PWA: Host static `frontend` on Netlify, Vercel, or GitHub Pages (HTTPS required for PWA install).

## Default seeded users
- admin / Admin123!  (Admin)
- plumber1 / Plumber123! (Plumber)
- electric1 / Electric123! (Electrician)
- tenant1 / Tenant123! (Tenant)

## Notes
- This is a starter project. Review & harden authentication, secrets, HTTPS, and production DB before going live.