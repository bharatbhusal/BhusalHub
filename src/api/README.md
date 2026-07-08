# BhusalHub API

ASP.NET Core 9 Web API — the backend service for BhusalHub.

## Stack

- .NET 9.0
- ASP.NET Core (controllers, cookie auth, middleware)
- SQLite via Microsoft.Data.Sqlite + Dapper
- xunit + WebApplicationFactory (tests)

## Quick start

```bash
dotnet run --project BhusalHub.Api
```

API listens on `http://localhost:5000` by default.

## Project structure

```
BhusalHub.Api/          — controllers, middleware, startup, DI
BhusalHub.Core/         — interfaces, models, domain services
BhusalHub.Data/         — database initializer, SQLite connection factory
```

## Auth

Cookie-based authentication with 30-day sliding expiry.

| Endpoint             | Method | Description                                      |
| -------------------- | ------ | ------------------------------------------------ |
| `/api/auth/register` | POST   | Create account (username, password, displayName) |
| `/api/auth/login`    | POST   | Sign in                                          |
| `/api/auth/logout`   | POST   | Sign out                                         |
| `/api/auth/me`       | GET    | Current user (requires auth cookie)              |

## Endpoints

| Endpoint          | Method | Auth | Description     |
| ----------------- | ------ | ---- | --------------- |
| `/api/health`     | GET    | No   | Health check    |
| `/api/media`      | GET    | No   | List media      |
| `/api/media/{id}` | GET    | No   | Get media by ID |

## Tests

```bash
dotnet test ../../tests/bhusalhub-api/BhusalHub.Tests.csproj
```
