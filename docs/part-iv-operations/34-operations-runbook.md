# 34 — Operations Runbook

**Status:** Draft  
**Last Updated:** 2026-07-08  
**Owner:** Bharat Bhusal  
**References:** [19 - Monitoring & Self-Healing](../part-ii-architecture/19-monitoring-self-healing.md), [33 - Disaster Recovery](33-disaster-recovery.md)

---

## Purpose

Provide step-by-step procedures for common operational tasks and incident response. This is the playbook for running BhusalHub day-to-day.

## Scope

Daily operations, common issues, and their resolutions. Major disasters are covered in [Chapter 33](33-disaster-recovery.md).

## Service Management

### View All Services

```bash
docker compose ps
```

### View Logs

```bash
# All services
docker compose logs

# Specific service, follow mode
docker compose logs -f bh-api

# Last 50 lines
docker compose logs --tail=50 bh-api
```

### Restart a Service

```bash
docker compose restart bh-api
```

### Update a Service

```bash
docker compose pull bh-api
docker compose up -d bh-api
```

### Full Stack Restart

```bash
docker compose down
docker compose up -d
```

## Health Checks

### Quick Health Check

```bash
# Via Docker
docker compose ps

# Via API
curl -s http://localhost:8080/api/health | jq .

# Via Nginx (through proxy)
curl -s https://home.bharatbhusal.com/api/health | jq .
```

### Detailed Health

```bash
curl -s http://localhost:8080/api/admin/health | jq .
```

Expected output:
```json
{
  "services": {
    "api": { "status": "healthy" },
    "database": { "status": "healthy", "size_mb": 12.4 },
    "storage": { "status": "healthy", "used_gb": 42.1, "total_gb": 2000 }
  },
  "uptime_seconds": 604800
}
```

## Common Incidents

### Incident: API Not Responding

**Symptoms:** Gallery shows errors, `docker compose ps` shows `bh-api` as unhealthy or exited.

**Triage:**
1. Check logs: `docker compose logs --tail=50 bh-api`
2. Check database connectivity: `sqlite3 /mnt/ssd/db/bhusalhub.db "SELECT 1;"`

**Resolution:**
- If DB is fine: `docker compose restart bh-api`
- If DB is corrupted: Restore from backup (see [Chapter 33](33-disaster-recovery.md))

### Incident: Nginx Returns 502

**Symptoms:** Browser shows 502 Bad Gateway.

**Triage:**
1. Is API running? `docker compose ps bh-api`
2. Is API healthy? `curl http://bh-api:8080/api/health`

**Resolution:**
- Restart API: `docker compose restart bh-api`
- If API is healthy but Nginx still returns 502, restart Nginx: `docker compose restart bh-nginx`

### Incident: Upload Fails

**Symptoms:** Upload returns error or hangs.

**Triage:**
1. Check disk space: `df -h /mnt/ssd`
2. Check file permissions: `ls -la /mnt/ssd/media/`
3. Check Nginx error log: `docker compose logs bh-nginx | grep -i error`

**Resolution:**
- Disk full: Free space or increase capacity.
- Permission issue: `chown -R 1000:1000 /mnt/ssd/media/`
- File too large: Check `client_max_body_size` in Nginx config.

### Incident: Remote Access Not Working

**Symptoms:** Cannot reach `home.bharatbhusal.com` from outside the home network.

**Triage:**
1. Is tunnel running? `docker compose ps bh-tunnel`
2. Tunnel logs: `docker compose logs --tail=50 bh-tunnel`
3. Local access working? If yes, the platform is healthy.

**Resolution:**
- Restart tunnel: `docker compose restart bh-tunnel`
- If tunnel fails to authenticate: Re-run `cloudflared tunnel login`

### Incident: High Memory Usage

**Symptoms:** System slow, alert from dashboard.

**Triage:**
1. Check overall memory: `free -h`
2. Check container memory: `docker stats --no-stream`

**Resolution:**
- If a container exceeds its limit: Check for memory leak, restart container.
- If all containers are within limits but system is low: Check for non-Docker processes.

### Incident: SSD Full

**Symptoms:** Uploads fail, health check warning.

**Triage:**
1. Check disk usage: `df -h /mnt/ssd`
2. Find large directories: `du -sh /mnt/ssd/* | sort -rh`

**Resolution:**
- Delete unnecessary files.
- Move old media to external storage.
- Add a larger SSD (requires re-mount and restore).

## Scheduled Maintenance

### Weekly

```bash
# Check disk usage
df -h /mnt/ssd

# Check system updates
sudo apt update && sudo apt upgrade -y

# SSD TRIM
sudo fstrim /mnt/ssd
```

### Monthly

```bash
# Full system reboot test
sudo reboot
# Verify services come up automatically

# Check backup integrity
sqlite3 /mnt/ssd/backups/db/bhusalhub-latest.db "SELECT COUNT(*) FROM Media;"
```

### Quarterly

```bash
# Full backup to external drive
rsync -av /mnt/ssd/ /backup/external/bhusalhub/

# Check SD card health
sudo smartctl -a /dev/mmcblk0  # if supported

# Check SSD health
sudo smartctl -a /dev/sda
```

## Related Chapters

- [19 - Monitoring & Self-Healing](../part-ii-architecture/19-monitoring-self-healing.md)
- [33 - Disaster Recovery](33-disaster-recovery.md)
- [31 - Raspberry Pi Optimization](31-raspberry-pi-optimization.md)
