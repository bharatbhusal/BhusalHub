# Appendix A — Architecture Decision Records

**Status:** Draft  
**Last Updated:** 2026-07-08  
**Owner:** Bharat Bhusal

---

## Purpose

Record significant architectural decisions and their rationale. Each ADR captures a decision that has lasting impact on the project.

## ADR Index

| ID | Title | Status | Date |
|----|-------|--------|------|
| ADR-001 | SQLite over PostgreSQL | Accepted | 2026-07-08 |
| ADR-002 | Static SPA over SSR | Accepted | 2026-07-08 |
| ADR-003 | Cloudflare Tunnel over port forwarding | Accepted | 2026-07-08 |
| ADR-004 | Session auth over JWT | Accepted | 2026-07-08 |
| ADR-005 | ImageSharp over ImageMagick | Accepted | 2026-07-08 |
| ADR-006 | Synchronous over async thumbnail processing | Accepted | 2026-07-08 |
| ADR-007 | Single-node over multi-node | Accepted | 2026-07-08 |
| ADR-008 | Dapper over Entity Framework | Accepted | 2026-07-08 |

---

## ADR-001: SQLite over PostgreSQL

**Status:** Accepted  
**Date:** 2026-07-08

### Context

BhusalHub needs a database for metadata storage. The target hardware is a Raspberry Pi 4B with 2 GB RAM.

### Decision

Use SQLite instead of PostgreSQL.

### Rationale

- SQLite has no separate server process, saving ~80 MB RAM.
- Zero configuration and maintenance.
- WAL mode provides crash recovery.
- Sufficient for the target workload (single-node, single-writer, read-heavy).
- Entire database is a single file — trivial to back up.

### Trade-offs

- No concurrent writes. Acceptable because the only writer is the API server.
- Limited to single-node. Acceptable because multi-node is not in V1 scope.
- Full-text search is available via FTS5 extension.

### Alternatives Considered

- **PostgreSQL:** Rejected due to memory overhead (~80 MB for the server process).
- **MariaDB/MySQL:** Same concern as PostgreSQL, plus less ARM64 optimization.
- **LiteDB:** Considered (MongoDB-like, .NET-native), but SQLite has a larger ecosystem.

---

## ADR-002: Static SPA over SSR

**Status:** Accepted  
**Date:** 2026-07-08

### Context

The frontend must be lightweight and not consume RAM on the server.

### Decision

Use React + Vite to generate a static SPA served by Nginx, with no server-side rendering.

### Rationale

- No Node.js runtime in production (saves ~100 MB RAM).
- Vite produces efficient bundles (~150 KB gzipped).
- Nginx serves static files with minimal overhead.
- The target network (Gigabit Ethernet) has < 1 ms latency, making SSR unnecessary for performance.

### Trade-offs

- Slower initial page load compared to SSR.
- No server-side SEO (irrelevant for a private platform).

### Alternatives Considered

- **Next.js:** Rejected because SSR adds a Node.js runtime in production.
- **SvelteKit:** Considered, but React has a larger ecosystem and the author's experience.

---

## ADR-003: Cloudflare Tunnel over Port Forwarding

**Status:** Accepted  
**Date:** 2026-07-08

### Context

Remote access must be secure and not expose the Pi directly to the internet.

### Decision

Use Cloudflare Tunnel (cloudflared) instead of port forwarding.

### Rationale

- Zero open inbound ports.
- No public IP or dynamic DNS required.
- Works behind CGNAT.
- Free tier is sufficient for household traffic.
- Encrypted tunnel from Pi to Cloudflare edge.

### Trade-offs

- Internet and Cloudflare are required for remote access.
- Cloudflare can potentially see the domain of requests (but not decrypted content).

### Alternatives Considered

- **Port forwarding:** Rejected for security concerns.
- **WireGuard:** Requires port forwarding or public IP.
- **Tailscale:** Requires client on every remote device.
- **FRP:** Requires a VPS, adding cost and complexity.

---

## ADR-004: Session Auth over JWT

**Status:** Accepted  
**Date:** 2026-07-08

### Context

Users must authenticate to access the platform. The auth mechanism must work offline.

### Decision

Use server-side session cookies instead of JWT.

### Rationale

- Sessions are simpler for a single-server deployment.
- No token refresh complexity.
- Session revocation is instant (delete from database).
- Works offline — no external identity provider needed.
- HttpOnly cookies are immune to XSS token theft.

### Trade-offs

- Not suitable for multi-server deployments without a shared session store.
- Slightly more server-side state than JWT.

### Alternatives Considered

- **JWT:** Rejected due to token refresh complexity and inability to instantly revoke tokens.
- **ASP.NET Core Identity:** Too heavy for the use case (adds ~50 tables).
- **OAuth2/OIDC:** Overkill for a household platform.

---

## ADR-005: ImageSharp over ImageMagick

**Status:** Accepted  
**Date:** 2026-07-08

### Context

Thumbnail generation requires image processing on the server.

### Decision

Use SixLabors.ImageSharp instead of ImageMagick.

### Rationale

- Pure .NET library — no native dependencies or external binaries (for images).
- Well-maintained, cross-platform (ARM64, x86_64).
- Good balance of performance and memory usage.
- Integrates naturally with the ASP.NET Core stack.

### Trade-offs

- Limited format support compared to ImageMagick (no HEIC, SVG).
- Slower than libvips for bulk processing.

### Alternatives Considered

- **ImageMagick:** Requires native binary, adds deployment complexity.
- **libvips:** Faster but requires native binary and P/Invoke.
- **SkiaSharp:** Good but adds native dependency.

---

## ADR-006: Synchronous over Async Thumbnail Processing

**Status:** Accepted  
**Date:** 2026-07-08

### Context

Thumbnails must be generated when media is uploaded. The choice is between synchronous (upload blocks until thumbnail is ready) and async (upload returns immediately, thumbnail generated in background).

### Decision

Process thumbnails synchronously during upload.

### Rationale

- Simpler implementation (no message queue, no background worker).
- Thumbnails are available immediately after upload.
- For household upload volume (tens per day), the delay is acceptable.
- The upload API call already takes time for file transfer; adding 1 second for thumbnailing is negligible.

### Trade-offs

- Slower upload completion for large videos (up to 3 seconds).
- Not suitable for high-volume uploads.

### Alternatives Considered

- **Background job with message queue:** Rejected as over-engineering for V1. A queue adds Redis or RabbitMQ, consuming ~50 MB RAM.

---

## ADR-007: Single-Node over Multi-Node

**Status:** Accepted  
**Date:** 2026-07-08

### Context

The platform must serve a household. The question is whether to design for horizontal scaling from the start.

### Decision

Design for a single node (Raspberry Pi 4B). Do not invest in multi-node capabilities.

### Rationale

- The target workload (3 users, 2 TB storage) fits on a single Pi 4B.
- Multi-node adds significant complexity: load balancer, shared storage, shared session store, data replication.
- Vertical scaling (RPi 5, x86 NUC) is simpler and sufficient.
- A single-node architecture is simpler to deploy, operate, and debug.

### Trade-offs

- Hardware failure = complete downtime (mitigated by backup + restore procedures).
- Requires migration effort to move to multi-node if needed later.

### Alternatives Considered

- **Multi-node Docker Swarm:** Over-engineered for V1.
- **Kubernetes (k3s):** Over-engineered. k3s alone uses ~200 MB RAM.

---

## ADR-008: Dapper over Entity Framework

**Status:** Accepted  
**Date:** 2026-07-08

### Context

The API needs a data access strategy for SQLite.

### Decision

Use Dapper (micro-ORM) instead of Entity Framework Core.

### Rationale

- Better performance — no change tracking, no proxy generation.
- Full SQL control — no unexpected queries.
- ~20 MB smaller deployment size.
- Simpler to reason about for a small database.
- Dapper is a single file — minimal abstraction.

### Trade-offs

- More manual SQL writing.
- No migrations tooling (EF Core has built-in migrations).
- No LINQ for complex queries.

### Mitigation

- SQLite migrations are written as plain SQL files, executed in order.
- Complex queries are rare (single-table reads, simple joins).

### Alternatives Considered

- **Entity Framework Core:** Rejected due to memory overhead and deployment size.
- **Raw ADO.NET:** Viable but Dapper adds just enough convenience (auto-mapping) without the weight of EF Core.
- **Linq2db:** Considered but less mature.
