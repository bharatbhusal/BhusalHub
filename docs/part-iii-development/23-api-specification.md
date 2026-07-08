# 23 — API Specification

**Status:** Draft  
**Last Updated:** 2026-07-08  
**Owner:** Bharat Bhusal  
**References:** [21 - Backend Architecture](21-backend-architecture.md), [Appendix D - API Reference](../appendices/d-api-reference.md)

---

## Purpose

Define the REST API contract between the frontend and backend. This specification is the source of truth for API behavior.

## Scope

All endpoints exposed by the ASP.NET Core API. This chapter covers design conventions and key endpoints. The full reference is in [Appendix D](../appendices/d-api-reference.md).

## API Conventions

| Convention | Standard |
|------------|----------|
| Base URL | `/api/` |
| Format | JSON (all requests and responses) |
| Authentication | Cookie-based (`bh_session`) |
| Pagination | `?page=1&pageSize=30` |
| Sorting | `?sort=created_at&order=desc` |
| Searching | `?q=search+term` |
| Errors | Problem Details (RFC 7807) |
| Versioning | URL prefix (`/api/v1/` in future) |

## Pagination

All list endpoints return:

```json
{
  "items": [ ... ],
  "total": 142,
  "page": 1,
  "pageSize": 30,
  "totalPages": 5
}
```

## Standard Error Response

```json
{
  "type": "https://httpstatuses.io/404",
  "title": "Not Found",
  "status": 404,
  "detail": "Media with ID 'abc-123' was not found.",
  "instance": "/api/media/abc-123"
}
```

## Endpoints

### Authentication

| Method | Path | Auth | Description |
|--------|------|------|-------------|
| POST | `/api/auth/login` | No | Authenticate user |
| POST | `/api/auth/logout` | Yes | End session |
| GET | `/api/auth/me` | Yes | Current user info |
| POST | `/api/setup` | No | First-time admin setup |

### Media

| Method | Path | Auth | Description |
|--------|------|------|-------------|
| GET | `/api/media` | Yes | List media (paginated, filterable) |
| GET | `/api/media/{id}` | Yes | Get media details |
| POST | `/api/media/upload` | Yes | Upload file(s) |
| DELETE | `/api/media/{id}` | Yes | Delete media |
| GET | `/api/media/{id}/download` | Yes | Download original file |
| GET | `/api/media/{id}/stream` | Yes | Stream video (range requests) |

### Albums

| Method | Path | Auth | Description |
|--------|------|------|-------------|
| GET | `/api/albums` | Yes | List albums |
| POST | `/api/albums` | Yes | Create album |
| GET | `/api/albums/{id}` | Yes | Get album with contents |
| PUT | `/api/albums/{id}` | Yes | Update album |
| DELETE | `/api/albums/{id}` | Yes | Delete album |
| POST | `/api/albums/{id}/media` | Yes | Add media to album |
| DELETE | `/api/albums/{id}/media/{mediaId}` | Yes | Remove media from album |

### Files

| Method | Path | Auth | Description |
|--------|------|------|-------------|
| GET | `/api/files` | Yes | List files/directories |
| POST | `/api/files/upload` | Yes | Upload file to filesystem |
| POST | `/api/files/folder` | Yes | Create folder |
| DELETE | `/api/files` | Yes | Delete file/folder |
| GET | `/api/files/download` | Yes | Download file |

### Search

| Method | Path | Auth | Description |
|--------|------|------|-------------|
| GET | `/api/search` | Yes | Full-text search across media |

### Admin

| Method | Path | Auth | Description |
|--------|------|------|-------------|
| GET | `/api/admin/users` | Admin | List users |
| POST | `/api/admin/users` | Admin | Create user |
| PUT | `/api/admin/users/{id}` | Admin | Update user |
| DELETE | `/api/admin/users/{id}` | Admin | Delete user |
| GET | `/api/admin/health` | Admin | Detailed system health |
| POST | `/api/admin/thumbnails/regenerate` | Admin | Regenerate all thumbnails |

### Health

| Method | Path | Auth | Description |
|--------|------|------|-------------|
| GET | `/api/health` | No | Simple health check |

## Request/Response Examples

### POST /api/auth/login

**Request:**
```json
{
  "username": "bharat",
  "password": "my-secure-password"
}
```

**Response (200):**
```json
{
  "id": "u-abc123",
  "username": "bharat",
  "displayName": "Bharat",
  "role": "admin"
}
```

**Set-Cookie:** `bh_session=...; HttpOnly; Secure; SameSite=Strict; Path=/; Max-Age=86400`

### GET /api/media

**Response (200):**
```json
{
  "items": [
    {
      "id": "m-xyz789",
      "filenameOriginal": "vacation.jpg",
      "mediaType": "photo",
      "mimeType": "image/jpeg",
      "fileSize": 4520000,
      "width": 4000,
      "height": 3000,
      "thumbnailPath": "/thumbnails/photos/2026/07/xyz789_thumb.jpg",
      "createdAt": "2026-07-08T10:30:00Z"
    }
  ],
  "total": 142,
  "page": 1,
  "pageSize": 30,
  "totalPages": 5
}
```

### POST /api/media/upload

**Request:** `multipart/form-data` with field `file`.

**Response (201):**
```json
{
  "id": "m-xyz789",
  "filenameOriginal": "vacation.jpg",
  "mediaType": "photo",
  "fileSize": 4520000,
  "thumbnailPath": "/thumbnails/photos/2026/07/xyz789_thumb.jpg",
  "createdAt": "2026-07-08T10:30:00Z"
}
```

## Related Chapters

- [21 - Backend Architecture](21-backend-architecture.md)
- [Appendix D - API Reference](../appendices/d-api-reference.md)
- [24 - User Journeys](24-user-journeys.md)
