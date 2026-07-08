# 06 — Glossary

**Status:** Draft  
**Last Updated:** 2026-07-08  
**Owner:** Bharat Bhusal

---

## Purpose

Define terms used throughout this handbook. Consistent terminology prevents misunderstandings across documentation, code, and discussions.

## Terms

### A

| Term | Definition |
|------|------------|
| **ADR** | Architecture Decision Record. A document capturing an architectural decision, its context, and its rationale. |
| **Album** | A user-created collection of media items. Albums are logical groupings, not directory structures. |

### C

| Term | Definition |
|------|------------|
| **Cloudflare Tunnel** | An encrypted tunnel from the Raspberry Pi to the Cloudflare edge network. Enables remote access without open inbound ports. |
| **Container** | A Docker container running a single service (API, Nginx, etc.). |

### D

| Term | Definition |
|------|------------|
| **Dashboard** | The main landing page showing system status, recent activity, and navigation. |
| **Docker Compose** | A tool for defining and running multi-container Docker applications using a YAML file. |

### E

| Term | Definition |
|------|------------|
| **Entry Point** | The single URL through which all users access the platform: `https://home.bharatbhusal.com`. |

### H

| Term | Definition |
|------|------------|
| **Health Check** | An endpoint (`/health`) that reports whether a service is functioning correctly. Used by Docker for restart decisions. |

### L

| Term | Definition |
|------|------------|
| **Local Network** | The home LAN behind the router. Devices can communicate without internet access. |
| **Local DNS** | DNS resolution provided by the home router. Used for split DNS to resolve `home.bharatbhusal.com` to the internal IP. |

### M

| Term | Definition |
|------|------------|
| **Media** | Digital content served by the platform: photos and videos primarily, but also documents and arbitrary files. |
| **Metadata** | Data about media: filename, size, dimensions, date, EXIF data, tags. Stored in SQLite. |

### N

| Term | Definition |
|------|------------|
| **Nginx** | A high-performance web server used as a reverse proxy and static file server. |

### O

| Term | Definition |
|------|------------|
| **Offline First** | A design principle requiring all core features to function without internet access. |

### P

| Term | Definition |
|------|------------|
| **Personal Infrastructure Platform** | The product category for BhusalHub. A platform that provides self-hosted infrastructure services (media, files, monitoring, applications) through a unified interface. |
| **Platform** | The entire BhusalHub system, including all services, infrastructure, and configuration. |

### R

| Term | Definition |
|------|------------|
| **Raspberry Pi 4B** | The target hardware. A single-board computer with a quad-core ARM Cortex-A72 CPU and 2 GB RAM. |
| **Reverse Proxy** | Nginx receives incoming requests and routes them to the appropriate backend service. |
| **RFC 2119** | A specification defining the meaning of MUST, SHOULD, MAY etc. in technical documentation. |

### S

| Term | Definition |
|------|------------|
| **Service** | A self-contained component of the platform (API, Nginx, tunnel, etc.), typically running in its own container. |
| **Split DNS** | A DNS configuration where `home.bharatbhusal.com` resolves to different IPs depending on whether the request originates inside or outside the home network. |
| **SQLite** | A self-contained, serverless database engine. Used as the primary datastore. |
| **SSD** | Solid-state drive. USB 3.0-connected, used for all persistent data. |

### T

| Term | Definition |
|------|------------|
| **Thumbnail** | A smaller, compressed version of an image or video frame, generated for efficient gallery browsing. |
| **Tunnel** | Short for Cloudflare Tunnel. See Cloudflare Tunnel. |

### U

| Term | Definition |
|------|------------|
| **User** | A person authenticated to use the platform. May be an admin or a family member. |

### W

| Term | Definition |
|------|------------|
| **WAL** | Write-Ahead Log. A SQLite journal mode that improves concurrent read performance and crash recovery. |
| **Watchtower** | A Docker container that automatically updates running containers to the latest image. |

## Related Chapters

All chapters reference glossary terms. This chapter is updated as new terms are introduced.
