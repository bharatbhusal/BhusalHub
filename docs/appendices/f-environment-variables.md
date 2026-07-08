# Appendix F — Environment Variables

**Status:** Draft  
**Last Updated:** 2026-07-08  
**Owner:** Bharat Bhusal  
**References:** [26 - Configuration](../part-iii-development/26-configuration.md)

---

## Purpose

Complete reference of all environment variables used by BhusalHub services.

## Scope

All environment variables across all services, including defaults and descriptions.

## API (`bh-api`)

| Variable | Default | Required | Description |
|----------|---------|----------|-------------|
| `BH_DB_PATH` | `/data/db/bhusalhub.db` | No | Path to SQLite database file |
| `BH_MEDIA_ROOT` | `/data/media` | No | Root directory for media file storage |
| `BH_THUMBNAIL_ROOT` | `/data/thumbnails` | No | Root directory for thumbnail storage |
| `BH_SESSION_EXPIRY_HOURS` | `24` | No | Session lifetime in hours |
| `BH_SESSION_MAX_HOURS` | `168` | No | Maximum total session lifetime (7 days) |
| `BH_LOG_LEVEL` | `Information` | No | Logging level: Debug, Information, Warning, Error |
| `BH_CORS_ORIGINS` | `http://localhost:5173` | No | Comma-separated allowed CORS origins |
| `BH_MAX_UPLOAD_SIZE_MB` | `4096` | No | Maximum upload size in MB |
| `BH_ADMIN_USERNAME` | — | Yes* | Initial admin username (first-time setup) |
| `BH_ADMIN_PASSWORD` | — | Yes* | Initial admin password (first-time setup) |
| `ASPNETCORE_URLS` | `http://0.0.0.0:8080` | No | API listening address and port |
| `ASPNETCORE_ENVIRONMENT` | `Production` | No | ASP.NET Core environment |

*\*Required for automated first-time setup. If not set, setup is done via the web UI.*

## Nginx (`bh-nginx`)

Nginx is configured via files, not environment variables.

| File | Mount Path | Description |
|------|------------|-------------|
| `nginx.conf` | `/etc/nginx/nginx.conf` | Main Nginx configuration |
| `ssl/cert.pem` | `/etc/nginx/ssl/cert.pem` | TLS certificate |
| `ssl/key.pem` | `/etc/nginx/ssl/key.pem` | TLS private key |

## Tunnel (`bh-tunnel`)

| File | Mount Path | Description |
|------|------------|-------------|
| `tunnel/config.yml` | `/etc/cloudflared/config.yml` | Tunnel configuration |
| `tunnel/credentials.json` | `/etc/cloudflared/credentials.json` | Tunnel credentials |

## Watchtower (`bh-watchtower`)

| Variable | Default | Required | Description |
|----------|---------|----------|-------------|
| `WATCHTOWER_CLEANUP` | `true` | No | Remove old images after update |
| `WATCHTOWER_SCHEDULE` | `0 0 4 * * *` | No | Cron schedule for updates (daily at 4 AM) |
| `WATCHTOWER_INCLUDE_RESTARTING` | `true` | No | Include restarting containers |
| `WATCHTOWER_TIMEOUT` | `30s` | No | Stop timeout during update |
| `WATCHTOWER_NO_STARTUP_MESSAGE` | `true` | No | Suppress startup message |

## Docker Compose Environment File Template

```bash
# /mnt/ssd/config/env/api.env

# BhusalHub API Configuration
BH_DB_PATH=/data/db/bhusalhub.db
BH_MEDIA_ROOT=/data/media
BH_THUMBNAIL_ROOT=/data/thumbnails
BH_SESSION_EXPIRY_HOURS=24
BH_SESSION_MAX_HOURS=168
BH_LOG_LEVEL=Information
BH_MAX_UPLOAD_SIZE_MB=4096

# ASP.NET Core
ASPNETCORE_URLS=http://0.0.0.0:8080
ASPNETCORE_ENVIRONMENT=Production
```
