# 24 — User Journeys

**Status:** Draft  
**Last Updated:** 2026-07-08  
**Owner:** Bharat Bhusal  
**References:** [02 - Functional Requirements](../part-i-foundation/02-functional-requirements.md), [23 - API Specification](23-api-specification.md)

---

## Purpose

Document the key user journeys to validate that the architecture supports real-world usage patterns.

## Scope

Primary workflows for Version 1. Edge cases and error states are documented inline.

## Journey 1: Browse Gallery

```mermaid
sequenceDiagram
    participant U as User
    participant N as Nginx
    participant A as API
    participant DB as SQLite

    U->>N: GET /gallery
    N-->>U: Serve SPA (index.html)
    U->>N: GET /api/media?page=1
    N->>A: Proxy request
    A->>DB: SELECT ... FROM Media ORDER BY created_at DESC LIMIT 30
    DB-->>A: 30 media records
    A-->>U: JSON response (paginated)
    U->>U: Render thumbnail grid
    U->>N: GET /thumbnails/photos/2026/07/thumb1.jpg
    N-->>U: Serve thumbnail file
    Note over U: User scrolls down
    U->>A: GET /api/media?page=2 (intersection observer)
    A-->>U: Next page
```

> **Caption:** Gallery browsing. The SPA loads immediately. Thumbnails are served directly by Nginx. Infinite scroll triggers paginated API calls.

## Journey 2: Upload Media

```mermaid
sequenceDiagram
    participant U as User
    participant N as Nginx
    participant A as API
    participant FS as Filesystem
    participant DB as SQLite

    U->>N: POST /api/media/upload (multipart)
    N->>A: Proxy request
    A->>A: Validate file (type, size, checksum)
    A->>A: Generate UUID filename
    A->>FS: Write file to /data/media/photos/2026/07/{uuid}.jpg
    A->>A: Generate thumbnail (ImageSharp)
    A->>FS: Write thumbnail
    A->>A: Extract metadata (dimensions, EXIF)
    A->>DB: INSERT INTO Media (...)
    DB-->>A: Media record created
    A-->>U: 201 Created (media JSON)
    U->>N: GET /api/media?page=1
    N->>A: Proxy request
    A-->>U: Updated gallery with new upload
```

> **Caption:** Upload flow. The file is validated, stored, thumbnailed, and indexed in a single request.

## Journey 3: Remote Access

```mermaid
sequenceDiagram
    participant RU as Remote User
    participant CF as Cloudflare
    participant T as cloudflared
    participant N as Nginx
    participant A as API

    RU->>CF: https://home.bharatbhusal.com/gallery
    CF->>T: Forward via tunnel
    T->>N: HTTP localhost:80/gallery
    N-->>T: Static SPA
    T-->>CF: Response
    CF-->>RU: HTTPS response

    Note over RU: Already authenticated (session cookie)
    RU->>CF: GET /api/media (with cookie)
    CF->>T: Forward
    T->>N: Proxy to API
    N->>A: Validate session
    A-->>N: Media list
    N-->>T: Response
    T-->>CF: Forward
    CF-->>RU: JSON response
```

> **Caption:** Remote access. The same SPA and API are served via Cloudflare Tunnel. The session cookie persists across requests.

## Journey 4: Power Recovery

```mermaid
sequenceDiagram
    participant PSU as Power
    participant RPI as Raspberry Pi
    participant D as Docker
    participant C as Containers

    Note over PSU,C: Power restored
    PSU->>RPI: Power on
    RPI->>RPI: POST (auto-login enabled)
    RPI->>RPI: BIOS -> Bootloader
    RPI->>RPI: Raspberry Pi OS boots
    RPI->>D: systemd starts Docker daemon
    D->>D: Docker daemon initializes
    D->>C: Docker Compose starts services
    C->>C: bh-api starts (health check OK)
    C->>C: bh-nginx starts (depends on API)
    Note over C: Platform operational (~90s total)
```

> **Caption:** Power recovery sequence. No manual intervention required. The platform is operational within ~90 seconds of power restoration.

## Journey 5: Search Media

```mermaid
sequenceDiagram
    participant U as User
    participant A as API
    participant DB as SQLite

    U->>A: GET /api/search?q=vacation+2026
    A->>DB: SELECT ... WHERE filename_original LIKE '%vacation%' AND (media_type = 'photo' OR media_type = 'video')
    A->>DB: Full-text search on tags
    DB-->>A: Matching records
    A-->>U: JSON response with matches
    U->>U: Browse search results
```

> **Caption:** Search flow. The search query is matched against filenames and tags. Results are returned as a paginated list.

## Journey 6: Create Album

```mermaid
sequenceDiagram
    participant U as User
    participant A as API
    participant DB as SQLite

    U->>A: POST /api/albums { name: "Summer 2026", description: "..." }
    A->>DB: INSERT INTO Album (...)
    DB-->>A: Album created
    A-->>U: 201 Created (album JSON)
    U->>A: POST /api/albums/{id}/media { mediaIds: [...] }
    A->>DB: INSERT INTO AlbumMedia (...)
    DB-->>A: Media added
    A-->>U: 200 OK
    U->>A: GET /api/albums/{id}
    A->>DB: SELECT album + joined media
    DB-->>A: Album with media list
    A-->>U: Album detail view
```

> **Caption:** Album creation and population. An album is created first, then media is added in a separate request.

## Related Chapters

- [02 - Functional Requirements](../part-i-foundation/02-functional-requirements.md)
- [23 - API Specification](23-api-specification.md)
