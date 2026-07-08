# BhusalHub Engineering Handbook

**Version:** 1.0.0  
**Status:** Draft  
**Owner:** Bharat Bhusal  
**Last Updated:** 2026-07-08

---

## Purpose

This handbook is the single source of truth for BhusalHub's architecture, infrastructure, development practices, and operations. Every engineer or AI agent working on this project MUST read this handbook before writing code.

## Scope

This handbook covers the entire BhusalHub platform: vision, system architecture, deployment, development guidelines, operational runbooks, and future roadmap. It does not document third-party dependencies beyond their integration with BhusalHub.

## How to Use This Handbook

| If you want to... | Start here |
|---|---|
| Understand the project's purpose | [00 - Project Charter](part-0-project-charter/00-project-charter.md) |
| Understand the system at a high level | [07 - System Context](part-ii-architecture/07-system-context.md) |
| Set up a development environment | [26 - Configuration](part-iii-development/26-configuration.md) |
| Deploy the platform | [09 - Deployment Architecture](part-ii-architecture/09-deployment-architecture.md) |
| Operate and monitor | [34 - Operations Runbook](part-iv-operations/34-operations-runbook.md) |
| Find a specific API endpoint | [Appendix D - API Reference](appendices/d-api-reference.md) |

---

## Table of Contents

### Part 0 — Project Charter

| # | Chapter | Status |
|---|---------|--------|
| 00 | [Project Charter](part-0-project-charter/00-project-charter.md) | Draft |

### Part I — Foundation

| # | Chapter | Status |
|---|---------|--------|
| 01 | [Vision](part-i-foundation/01-vision.md) | Draft |
| 02 | [Functional Requirements](part-i-foundation/02-functional-requirements.md) | Draft |
| 03 | [Non-Functional Requirements](part-i-foundation/03-non-functional-requirements.md) | Draft |
| 04 | [Architecture Principles](part-i-foundation/04-architecture-principles.md) | Draft |
| 05 | [Technology Decisions](part-i-foundation/05-technology-decisions.md) | Draft |
| 06 | [Glossary](part-i-foundation/06-glossary.md) | Draft |

### Part II — Architecture

| # | Chapter | Status |
|---|---------|--------|
| 07 | [System Context](part-ii-architecture/07-system-context.md) | Draft |
| 08 | [High-Level Design](part-ii-architecture/08-high-level-design.md) | Draft |
| 09 | [Deployment Architecture](part-ii-architecture/09-deployment-architecture.md) | Draft |
| 10 | [Network Architecture](part-ii-architecture/10-network-architecture.md) | Draft |
| 11 | [Service Architecture](part-ii-architecture/11-service-architecture.md) | Draft |
| 12 | [Docker Architecture](part-ii-architecture/12-docker-architecture.md) | Draft |
| 13 | [Storage Architecture](part-ii-architecture/13-storage-architecture.md) | Draft |
| 14 | [Database Architecture](part-ii-architecture/14-database-architecture.md) | Draft |
| 15 | [Authentication](part-ii-architecture/15-authentication.md) | Draft |
| 16 | [DNS Strategy](part-ii-architecture/16-dns-strategy.md) | Draft |
| 17 | [Tunnel Architecture](part-ii-architecture/17-tunnel-architecture.md) | Draft |
| 18 | [Security Architecture](part-ii-architecture/18-security-architecture.md) | Draft |
| 19 | [Monitoring & Self-Healing](part-ii-architecture/19-monitoring-self-healing.md) | Draft |

### Part III — Development

| # | Chapter | Status |
|---|---------|--------|
| 20 | [Frontend Architecture](part-iii-development/20-frontend-architecture.md) | Draft |
| 21 | [Backend Architecture](part-iii-development/21-backend-architecture.md) | Draft |
| 22 | [Thumbnail Processing](part-iii-development/22-thumbnail-processing.md) | Draft |
| 23 | [API Specification](part-iii-development/23-api-specification.md) | Draft |
| 24 | [User Journeys](part-iii-development/24-user-journeys.md) | Draft |
| 25 | [UI Architecture](part-iii-development/25-ui-architecture.md) | Draft |
| 26 | [Configuration](part-iii-development/26-configuration.md) | Draft |
| 27 | [Folder Structure](part-iii-development/27-folder-structure.md) | Draft |

### Part IV — Operations

| # | Chapter | Status |
|---|---------|--------|
| 28 | [Startup Lifecycle](part-iv-operations/28-startup-lifecycle.md) | Draft |
| 29 | [Request Lifecycle](part-iv-operations/29-request-lifecycle.md) | Draft |
| 30 | [Performance](part-iv-operations/30-performance.md) | Draft |
| 31 | [Raspberry Pi Optimization](part-iv-operations/31-raspberry-pi-optimization.md) | Draft |
| 32 | [Scalability](part-iv-operations/32-scalability.md) | Draft |
| 33 | [Disaster Recovery](part-iv-operations/33-disaster-recovery.md) | Draft |
| 34 | [Operations Runbook](part-iv-operations/34-operations-runbook.md) | Draft |

### Part V — Future

| # | Chapter | Status |
|---|---------|--------|
| 35 | [Roadmap](part-v-future/35-roadmap.md) | Draft |
| 36 | [Future Extensions](part-v-future/36-future-extensions.md) | Draft |
| 37 | [Lessons Learned](part-v-future/37-lessons-learned.md) | Draft |

### Appendices

| # | Chapter | Status |
|---|---------|--------|
| A | [Architecture Decision Records](appendices/a-architecture-decision-records.md) | Draft |
| B | [Mermaid Diagrams](appendices/b-mermaid-diagrams.md) | Draft |
| C | [Database Schema](appendices/c-database-schema.md) | Draft |
| D | [API Reference](appendices/d-api-reference.md) | Draft |
| E | [Docker Compose](appendices/e-docker-compose.md) | Draft |
| F | [Environment Variables](appendices/f-environment-variables.md) | Draft |

---

## Document Conventions

### RFC 2119 Language

The key words MUST, MUST NOT, REQUIRED, SHALL, SHALL NOT, SHOULD, SHOULD NOT, RECOMMENDED, MAY, and OPTIONAL in this handbook are to be interpreted as described in [RFC 2119](https://datatracker.ietf.org/doc/html/rfc2119).

### Diagram Standards

- All diagrams use Mermaid syntax.
- Diagrams MUST be renderable in any Mermaid-compatible viewer (GitHub, Markdown editors).
- Every diagram MUST have a caption.
- Labels with special characters MUST be quoted: `U["User"]`.

### Cross-References

Chapters are referenced by number and title: `[Chapter 07 - System Context](part-ii-architecture/07-system-context.md)`.

### ADR References

Architecture Decision Records are referenced as `ADR-NNN` with a link: `[ADR-001](appendices/a-architecture-decision-records.md#adr-001)`.

### Versioning

This handbook follows SemVer 2.0.0:
- **Major**: Architectural changes that invalidate prior decisions.
- **Minor**: New chapters or significant additions.
- **Patch**: Corrections, clarifications, formatting.

---

## Lifecycle

This handbook is a living document. Every architectural change MUST be accompanied by corresponding handbook updates. Chapters MAY be updated independently, but the version number in `docs/README.md` reflects the handbook as a whole.
