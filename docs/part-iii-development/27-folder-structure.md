# 27 — Folder Structure

**Status:** Draft  
**Last Updated:** 2026-07-08  
**Owner:** Bharat Bhusal  
**References:** [00 - Project Charter](../part-0-project-charter/00-project-charter.md), [21 - Backend Architecture](21-backend-architecture.md), [20 - Frontend Architecture](20-frontend-architecture.md)

---

## Purpose

Define the canonical repository layout for BhusalHub. This structure is the ground truth — every directory and file has a defined purpose.

## Scope

The entire repository, from top-level directories to source files. All new code MUST follow this structure.

## Repository Layout

```
BhusalHub/
├── README.md                    # Project overview, badges, quick start
├── LICENSE                      # License file
├── .gitignore                   # Git ignore rules
├── .github/
│   └── workflows/
│       ├── ci.yml               # CI pipeline (build, test, lint)
│       └── build-api.yml        # Build and push API Docker image
│
├── docs/                        # Engineering handbook
│   ├── README.md                # Handbook index and navigation
│   ├── templates/
│   │   └── chapter-template.md  # Reusable chapter template
│   ├── part-0-project-charter/
│   │   ├── 00-project-charter.md
│   ├── part-i-foundation/
│   │   ├── 01-vision.md
│   │   ├── 02-functional-requirements.md
│   │   ├── 03-non-functional-requirements.md
│   │   ├── 04-architecture-principles.md
│   │   ├── 05-technology-decisions.md
│   │   └── 06-glossary.md
│   ├── part-ii-architecture/    # 07-19 (see handbook index)
│   ├── part-iii-development/    # 20-27 (see handbook index)
│   ├── part-iv-operations/      # 28-34 (see handbook index)
│   ├── part-v-future/           # 35-37 (see handbook index)
│   └── appendices/              # A-F (see handbook index)
│
├── src/
│   ├── frontend/                # React + Vite SPA
│   │   ├── index.html           # SPA entry point
│   │   ├── vite.config.ts       # Vite configuration
│   │   ├── tsconfig.json        # TypeScript config
│   │   ├── package.json
│   │   ├── public/              # Static assets (favicon, robots.txt)
│   │   ├── src/
│   │   │   ├── main.tsx         # Application entry point
│   │   │   ├── App.tsx          # Root component, router setup
│   │   │   ├── api/             # API client, fetch wrappers
│   │   │   │   └── client.ts
│   │   │   ├── components/      # Shared/reusable components
│   │   │   │   ├── Layout/
│   │   │   │   ├── Header/
│   │   │   │   ├── Sidebar/
│   │   │   │   ├── ThumbnailCard/
│   │   │   │   ├── Modal/
│   │   │   │   ├── LoadingSpinner/
│   │   │   │   ├── ErrorState/
│   │   │   │   ├── EmptyState/
│   │   │   │   └── SearchBar/
│   │   │   ├── pages/           # Page-level components (one per route)
│   │   │   │   ├── Login/
│   │   │   │   ├── Dashboard/
│   │   │   │   ├── Gallery/
│   │   │   │   ├── MediaView/
│   │   │   │   ├── Albums/
│   │   │   │   ├── FileBrowser/
│   │   │   │   ├── Upload/
│   │   │   │   └── Admin/
│   │   │   ├── hooks/           # Custom React hooks
│   │   │   │   ├── useApi.ts
│   │   │   │   └── useAuth.ts
│   │   │   ├── context/         # React Context providers
│   │   │   │   └── AuthContext.tsx
│   │   │   ├── types/           # TypeScript type definitions
│   │   │   │   └── index.ts
│   │   │   └── styles/          # Global styles, CSS variables
│   │   │       └── globals.css
│   │   └── tests/               # Vitest + Testing Library tests
│   │       ├── components/
│   │       └── pages/
│   │
│   └── backend/                 # ASP.NET Core Web API
│       ├── BhusalHub.sln        # Solution file
│       ├── BhusalHub.Api/       # API project (controllers, middleware)
│       │   ├── BhusalHub.Api.csproj
│       │   ├── Program.cs       # Host builder, middleware pipeline
│       │   ├── Controllers/
│       │   │   ├── AuthController.cs
│       │   │   ├── MediaController.cs
│       │   │   ├── AlbumsController.cs
│       │   │   ├── FilesController.cs
│       │   │   ├── AdminController.cs
│       │   │   └── HealthController.cs
│       │   ├── Middleware/
│       │   │   ├── ExceptionHandlingMiddleware.cs
│       │   │   ├── RequestLoggingMiddleware.cs
│       │   │   └── SessionValidationMiddleware.cs
│       │   ├── Filters/
│       │   │   ├── AuthenticationFilter.cs
│       │   │   └── AdminAuthorizationFilter.cs
│       │   ├── DTOs/
│       │   │   ├── MediaDto.cs
│       │   │   ├── LoginRequest.cs
│       │   │   └── UploadResponse.cs
│       │   └── appsettings.json
│       ├── BhusalHub.Core/      # Core project (models, services)
│       │   ├── BhusalHub.Core.csproj
│       │   ├── Models/
│       │   │   ├── Media.cs
│       │   │   ├── User.cs
│       │   │   ├── Album.cs
│       │   │   ├── Session.cs
│       │   │   └── Tag.cs
│       │   ├── Services/
│       │   │   ├── MediaService.cs
│       │   │   ├── AlbumService.cs
│       │   │   ├── AuthService.cs
│       │   │   ├── ThumbnailService.cs
│       │   │   └── SearchService.cs
│       │   └── Interfaces/
│       │       ├── IMediaService.cs
│       │       ├── IAlbumService.cs
│       │       ├── IAuthService.cs
│       │       └── IThumbnailService.cs
│       └── BhusalHub.Data/      # Data project (repositories, migrations)
│           ├── BhusalHub.Data.csproj
│           ├── DatabaseInitializer.cs
│           ├── Migrations/
│           │   ├── Migration_001_InitialSchema.cs
│           │   └── Migration_002_AddTags.cs
│           └── Repositories/
│               ├── MediaRepository.cs
│               ├── UserRepository.cs
│               ├── AlbumRepository.cs
│               └── SessionRepository.cs
│
├── infra/
│   ├── docker/
│   │   ├── Dockerfile.api       # Multi-stage Dockerfile for ASP.NET Core
│   │   └── Dockerfile.nginx     # Custom Nginx image (if needed, else use stock)
│   ├── compose/
│   │   ├── docker-compose.yml   # Main Compose file
│   │   ├── docker-compose.remote.yml  # Override for tunnel
│   │   └── docker-compose.updates.yml # Override for watchtower
│   ├── nginx/
│   │   └── nginx.conf           # Nginx configuration template
│   └── scripts/
│       ├── setup.sh             # Initial setup script (OS + Docker + SSD mount)
│       └── backup.sh            # Database backup script
│
├── tests/
│   ├── backend/                 # Backend integration tests
│   │   ├── BhusalHub.Tests.csproj
│   │   ├── Controllers/
│   │   ├── Services/
│   │   └── Repositories/
│   └── frontend/                # Frontend tests (optional, tests live in src/frontend/tests)
│
└── scripts/                     # Development scripts
    ├── dev-up.sh                # Start development environment
    ├── dev-down.sh              # Stop development environment
    └── lint.sh                  # Run linters across all projects
```

## Directory Purpose Summary

| Directory | Purpose | Owner |
|-----------|---------|-------|
| `docs/` | Engineering handbook | All contributors |
| `src/frontend/` | React SPA | Frontend |
| `src/backend/` | ASP.NET Core API | Backend |
| `infra/docker/` | Dockerfiles | DevOps |
| `infra/compose/` | Docker Compose files | DevOps |
| `infra/nginx/` | Nginx configuration | DevOps |
| `infra/scripts/` | Deployment/ops scripts | DevOps |
| `tests/` | Integration tests | QA |
| `scripts/` | Developer tooling | All contributors |

## File Naming Conventions

| Artifact | Convention | Example |
|----------|------------|---------|
| C# classes | PascalCase | `MediaController.cs` |
| C# interfaces | I + PascalCase | `IMediaService.cs` |
| TypeScript files | camelCase | `useApi.ts` |
| React components | PascalCase | `ThumbnailCard.tsx` |
| CSS Modules | camelCase + .module.css | `thumbnailCard.module.css` |
| Dockerfiles | PascalCase with dot | `Dockerfile.api` |
| Compose files | kebab-case | `docker-compose.yml` |
| Scripts | kebab-case | `setup.sh` |
| Migrations | Prefix-001 + PascalCase | `Migration_001_InitialSchema.cs` |

## Related Chapters

- [00 - Project Charter](../part-0-project-charter/00-project-charter.md)
- [21 - Backend Architecture](21-backend-architecture.md)
- [20 - Frontend Architecture](20-frontend-architecture.md)
