# 31 — Raspberry Pi Optimization

**Status:** Draft  
**Last Updated:** 2026-07-08  
**Owner:** Bharat Bhusal  
**References:** [30 - Performance](30-performance.md), [12 - Docker Architecture](../part-ii-architecture/12-docker-architecture.md)

---

## Purpose

Document the specific optimizations required to run BhusalHub reliably on a Raspberry Pi 4B with 2 GB RAM.

## Scope

OS-level, Docker-level, and application-level optimizations for the Raspberry Pi.

## OS Configuration

### Reduce Memory Usage

```
# /boot/config.txt
gpu_mem=16                # Minimum GPU memory (headless)
arm_64bit=1               # 64-bit kernel
arm_freq=1500             # Default frequency (thermal management)
```

### Disable Unnecessary Services

```bash
sudo systemctl disable bluetooth.service
sudo systemctl disable hciuart.service
sudo systemctl disable avahi-daemon.service
sudo systemctl disable triggerhappy.service
sudo systemctl disable cron.service  # if not needed
```

### Swap

Disable swap to prevent SD card wear and unpredictable performance:

```bash
sudo dphys-swapfile swapoff
sudo dphys-swapfile uninstall
sudo systemctl disable dphys-swapfile
```

### Filesystem Mount Options

```
/dev/sda1  /mnt/ssd  ext4  defaults,noatime,nodiratime  0  2
```

| Option | Benefit |
|--------|---------|
| `noatime` | Avoids write on every read (reduces write amplification on SSD) |
| `nodiratime` | Same as noatime but for directories |

### SSD TRIM

Weekly fstrim for SSD health:

```bash
sudo systemctl enable fstrim.timer
```

## Docker Optimization

### Resource Limits

Every container MUST specify memory limits:

```yaml
services:
  bh-api:
    deploy:
      resources:
        limits: { memory: 256M }
        reservations: { memory: 128M }
```

- `limits` — Hard cap. Container is OOM-killed if exceeded.
- `reservations` — Guaranteed minimum. Docker scheduler uses this.

### Log Rotation

```json
{
  "log-driver": "json-file",
  "log-opts": {
    "max-size": "10m",
    "max-file": "3"
  }
}
```

Configured in `/etc/docker/daemon.json`. Prevents logs from filling the SD card.

### Image Strategy

- Alpine-based images wherever possible.
- ASP.NET Core published as trimmed self-contained deployment.
- Multi-stage builds to minimize image size.

## Application Optimization

### ASP.NET Core

```csharp
// Program.cs
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 4L * 1024 * 1024 * 1024; // 4 GB
    options.Limits.MaxConcurrentConnections = 100;
});

// Enable response compression
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});
```

### SQLite

WAL mode reduces write amplification and improves concurrent read performance:

```sql
PRAGMA journal_mode = WAL;
PRAGMA synchronous = NORMAL;
PRAGMA cache_size = -8000;
PRAGMA mmap_size = 268435456;
```

## Thermal Management

The Raspberry Pi 4B throttles at 80°C. Passive cooling (heatsink + case ventilation) is RECOMMENDED.

| Configuration | Max Performance | Notes |
|--------------|-----------------|-------|
| Stock (no heatsink) | Throttles under sustained load | Unacceptable for thumbnail generation |
| Heatsink only | Better but may throttle | Minimum viable |
| Heatsink + fan | No throttling | RECOMMENDED |

Monitor temperature with `vcgencmd measure_temp`.

## SD Card Protection

The SD card is used for boot only. All application data is on the USB SSD.

| Protection | Implementation |
|------------|----------------|
| Minimize writes | `/var/log` → tmpfs (in-memory) |
| Reduce fsync | SQLite on SSD, not SD card |
| Log rotation | Docker logs on SSD, not SD card |
| No swap | Disabled entirely |

## Health Checks for RPi

```bash
# CPU temperature
vcgencmd measure_temp

# CPU throttling status
vcgencmd get_throttled

# Memory usage
free -h

# Disk usage
df -h /mnt/ssd

# Docker resource usage
docker stats --no-stream
```

## Related Chapters

- [30 - Performance](30-performance.md)
- [12 - Docker Architecture](../part-ii-architecture/12-docker-architecture.md)
- [34 - Operations Runbook](34-operations-runbook.md)
