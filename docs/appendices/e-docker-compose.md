# Appendix E — Docker Compose

**Status:** Draft  
**Last Updated:** 2026-07-08  
**Owner:** Bharat Bhusal  
**References:** [12 - Docker Architecture](../part-ii-architecture/12-docker-architecture.md), [09 - Deployment Architecture](../part-ii-architecture/09-deployment-architecture.md)

---

## Purpose

Complete Docker Compose configuration for all BhusalHub services.

## Scope

The main `docker-compose.yml` file and profile overrides. See [Chapter 12](../part-ii-architecture/12-docker-architecture.md) for the architectural rationale.

## docker-compose.yml

```yaml
version: '3.8'

volumes:
  bh_media:
    driver: local
    driver_opts:
      type: none
      device: /mnt/ssd/media
      o: bind
  bh_thumbnails:
    driver: local
    driver_opts:
      type: none
      device: /mnt/ssd/thumbnails
      o: bind
  bh_db:
    driver: local
    driver_opts:
      type: none
      device: /mnt/ssd/db
      o: bind
  bh_config:
    driver: local
    driver_opts:
      type: none
      device: /mnt/ssd/config
      o: bind

networks:
  bh-network:
    driver: bridge
    ipam:
      config:
        - subnet: 172.18.0.0/16

services:
  bh-api:
    image: bhusalhub/api:latest
    build:
      context: ./src/backend
      dockerfile: ../../infra/docker/Dockerfile.api
    container_name: bh-api
    restart: unless-stopped
    networks:
      - bh-network
    volumes:
      - bh_media:/data/media
      - bh_thumbnails:/data/thumbnails
      - bh_db:/data/db
      - bh_config:/data/config:ro
    environment:
      - BH_DB_PATH=/data/db/bhusalhub.db
      - BH_MEDIA_ROOT=/data/media
      - BH_THUMBNAIL_ROOT=/data/thumbnails
      - BH_LOG_LEVEL=Information
      - ASPNETCORE_URLS=http://0.0.0.0:8080
    healthcheck:
      test: ["CMD", "wget", "--no-verbose", "--tries=1", "--spider", "http://localhost:8080/api/health"]
      interval: 30s
      timeout: 3s
      start_period: 15s
      retries: 3
    deploy:
      resources:
        limits:
          memory: 256M
        reservations:
          memory: 128M

  bh-nginx:
    image: nginx:alpine
    container_name: bh-nginx
    restart: unless-stopped
    networks:
      - bh-network
    ports:
      - "443:443"
      - "80:80"
    volumes:
      - bh_media:/data/media:ro
      - bh_thumbnails:/data/thumbnails:ro
      - ./infra/nginx/nginx.conf:/etc/nginx/nginx.conf:ro
      - bh_config:/etc/nginx/ssl:ro
    depends_on:
      bh-api:
        condition: service_healthy
    healthcheck:
      test: ["CMD", "wget", "--no-verbose", "--tries=1", "--spider", "http://localhost:80/health"]
      interval: 30s
      timeout: 3s
      start_period: 10s
      retries: 3
    deploy:
      resources:
        limits:
          memory: 64M
        reservations:
          memory: 32M

  bh-tunnel:
    image: cloudflare/cloudflared:latest
    container_name: bh-tunnel
    restart: unless-stopped
    networks:
      - bh-network
    command: tunnel run
    volumes:
      - bh_config:/etc/cloudflared:ro
    profiles:
      - remote-access
    deploy:
      resources:
        limits:
          memory: 64M
        reservations:
          memory: 32M

  bh-watchtower:
    image: containrrr/watchtower:latest
    container_name: bh-watchtower
    restart: unless-stopped
    volumes:
      - /var/run/docker.sock:/var/run/docker.sock
    environment:
      - WATCHTOWER_CLEANUP=true
      - WATCHTOWER_SCHEDULE=0 0 4 * * *
    profiles:
      - updates
    deploy:
      resources:
        limits:
          memory: 32M
        reservations:
          memory: 16M
```

## Usage

### Minimal deployment (no tunnel, no updates)

```bash
docker compose up -d
```

### With remote access

```bash
docker compose --profile remote-access up -d
```

### With auto-updates

```bash
docker compose --profile updates up -d
```

### Full stack

```bash
docker compose --profile remote-access --profile updates up -d
```

### Stop all services

```bash
docker compose down
```

### Update a specific service

```bash
docker compose pull bh-api
docker compose up -d bh-api
```
