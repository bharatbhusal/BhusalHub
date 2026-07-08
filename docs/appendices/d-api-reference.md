# Appendix D — API Reference

**Status:** Draft  
**Last Updated:** 2026-07-08  
**Owner:** Bharat Bhusal  
**References:** [23 - API Specification](../part-iii-development/23-api-specification.md)

---

## Purpose

Complete reference for all API endpoints. Generated from the API implementation (this appendix will be updated as the API evolves).

## Reference

*Note: This appendix will be automatically generated from the ASP.NET Core API using Swagger/OpenAPI during CI. The content below is the V1 target contract.*

For the most up-to-date API reference, run the API and visit `/swagger` or `/openapi/v1.json`.

## Endpoint Summary

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
| GET | `/api/media` | Yes | List media (paginated) |
| GET | `/api/media/{id}` | Yes | Get media details |
| POST | `/api/media/upload` | Yes | Upload file(s) |
| DELETE | `/api/media/{id}` | Yes | Delete media |
| GET | `/api/media/{id}/download` | Yes | Download original file |
| GET | `/api/media/{id}/stream` | Yes | Stream video |

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
| POST | `/api/files/upload` | Yes | Upload file |
| POST | `/api/files/folder` | Yes | Create folder |
| DELETE | `/api/files` | Yes | Delete file/folder |
| GET | `/api/files/download` | Yes | Download file |

### Search

| Method | Path | Auth | Description |
|--------|------|------|-------------|
| GET | `/api/search` | Yes | Full-text search |

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
