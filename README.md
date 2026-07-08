# BhusalHub

An offline-first, self-hosted personal infrastructure platform for Raspberry Pi.

**BhusalHub is not a media server — it is a Personal Infrastructure Platform.**  
Media management is the first capability built on top of that platform.

## Overview

BhusalHub transforms a Raspberry Pi 4B (2 GB RAM) into a resilient home cloud. It unifies media management, file access, and system monitoring behind a single secure web portal — no internet required for core functionality.

## Documentation

Full engineering handbook at [`docs/`](docs/README.md):

| Part | Contents |
|------|----------|
| [Part 0](docs/part-0-project-charter/00-project-charter.md) | Project Charter |
| [Part I](docs/part-i-foundation/01-vision.md) | Vision, Requirements, Principles, Tech Decisions |
| [Part II](docs/part-ii-architecture/07-system-context.md) | System Context through Monitoring |
| [Part III](docs/part-iii-development/20-frontend-architecture.md) | Frontend, Backend, API, Folder Structure |
| [Part IV](docs/part-iv-operations/28-startup-lifecycle.md) | Operations, Performance, Runbook |
| [Part V](docs/part-v-future/35-roadmap.md) | Roadmap & Future Extensions |
| [Appendices](docs/appendices/a-architecture-decision-records.md) | ADRs, Schema, API Ref, Docker Compose |

## Tech Stack

| Layer | Choice |
|-------|--------|
| Frontend | React + Vite (static SPA) |
| Backend | ASP.NET Core 8 |
| Database | SQLite |
| Proxy | Nginx |
| Runtime | Docker Compose |
| Remote access | Cloudflare Tunnel |
| Hardware | Raspberry Pi 4B, 2 GB RAM, USB 3 SSD |

## Quick Start

```bash
# Clone the repo and read the handbook first
git clone git@github.com:bharatbhusal/BhusalHub.git
cd BhusalHub
```

See [`docs/part-iv-operations/34-operations-runbook.md`](docs/part-iv-operations/34-operations-runbook.md) for deployment and daily operations.

## License

This project is licensed under the terms of the LICENSE file in this repository.
