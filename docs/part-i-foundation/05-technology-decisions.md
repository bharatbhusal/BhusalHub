# 05 — Technology Decisions

**Status:** Draft  
**Last Updated:** 2026-07-08  
**Owner:** Bharat Bhusal  
**References:** [04 - Architecture Principles](04-architecture-principles.md), [Appendix A](../appendices/a-architecture-decision-records.md)

---

## Purpose

Document the technology choices made for BhusalHub, including the rationale for each selection and alternatives that were rejected.

## Scope

This chapter covers major technology decisions. Trivial choices (e.g., specific npm packages) are documented in code or package manifests, not here.

## Decision Matrix

| Concern | Chosen | Rejected | Rationale |
|---------|--------|----------|-----------|
| Backend framework | ASP.NET Core 8 | Node.js, Python FastAPI, Go | Mature, excellent ARM64 perf, strong typing, single runtime |
| Frontend framework | React + Vite | Next.js, Svelte, Vue | Static generation, no SSR runtime in prod, broad ecosystem |
| Database | SQLite | PostgreSQL, MariaDB | Zero administration, no separate server process, fits in 2 GB RAM |
| Container runtime | Docker + Compose | Podman, k3s, Nomad | Lowest overhead, simplest orchestration, ARM64 native |
| Reverse proxy | Nginx | Caddy, Traefik, HAProxy | Proven ARM64 performance, simpler config for static use case |
| Remote access | Cloudflare Tunnel | WireGuard, Tailscale, FRP | Zero open ports, managed tunnel, free tier sufficient |
| Image updates | Watchtower | Renovate, manual | Automatic, configurable, minimal overhead |
| Media processing | ImageSharp + FFmpeg | ImageMagick, libvips | Native .NET integration (ImageSharp), FFmpeg for video |
| Auth mechanism | Cookie-based sessions | JWT, OAuth2, OIDC | Simpler for local-only use, no token refresh complexity |
| TLS termination | Nginx (self-managed) | Cloudflare origin certs, Let's Encrypt | Full control, works offline, no external dependency |

## Decision Details

### Backend: ASP.NET Core 8

**Rationale:**
- Native ARM64 support with excellent performance.
- Minimal memory footprint compared to Java/Node.js.
- Kestrel web server is production-grade and lightweight.
- C# provides strong typing and compile-time safety.
- Minimal API surface reduces boilerplate.

**Trade-off:** Smaller ecosystem for media-related libraries compared to Python. Chose ImageSharp for .NET-native image processing.

### Database: SQLite

**Rationale:**
- No database server process — saves ~100 MB RAM.
- Zero configuration, zero maintenance.
- WAL mode provides crash recovery without a separate server.
- Sufficient for single-node, single-writer workload.
- Entire database file is portable and backup-friendly.

**Trade-off:** No concurrent writes. This is acceptable because the workload is read-heavy and the only writer is the API server.

**Rejected — PostgreSQL:** Adds ~80 MB RAM for the server process, requires connection pooling, more complex backup. Overkill for a single-user/small-family workload.

### Frontend: React + Vite

**Rationale:**
- Vite produces pure static files — no Node.js runtime in production.
- React is the most widely used UI library, ensuring maintainability.
- Client-side routing eliminates server-side rendering complexity.
- Build-time code splitting minimizes initial load size.

**Trade-off:** No SSR means slightly slower initial page load. Acceptable on local network (Gigabit Ethernet, < 1 ms latency).

### Remote Access: Cloudflare Tunnel

**Rationale:**
- Zero open inbound ports — the tunnel initiates outbound connections.
- No dynamic DNS required.
- Free tier supports the expected traffic volume.
- Integrates with `home.bharatbhusal.com` DNS.
- No public IP required.

**Trade-off:** Internet connectivity is required for remote access. If Cloudflare is down, remote access is unavailable — local access is unaffected.

## When to Add a New Technology

A new dependency MAY be added only if:

1. It satisfies a principle from [Chapter 04](04-architecture-principles.md).
2. It does not conflict with the 2 GB RAM constraint.
3. It runs on ARM64.
4. It can be containerized.
5. No existing dependency provides equivalent functionality.
6. The team is willing to maintain it for the life of the project.

## Related Chapters

- [04 - Architecture Principles](04-architecture-principles.md)
- [Appendix A - ADRs](../appendices/a-architecture-decision-records.md)
