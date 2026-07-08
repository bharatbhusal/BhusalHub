# Appendix B — Mermaid Diagrams

**Status:** Draft  
**Last Updated:** 2026-07-08  
**Owner:** Bharat Bhusal

---

## Purpose

This appendix contains all Mermaid diagrams from the handbook in a single index for quick reference and validation.

---

### Chapter 00 — Identity Diagram

```mermaid
flowchart TD
    U["User"] --> D["home.bharatbhusal.com"]
    D --> P["BhusalHub Platform"]
    P --> M["Media"]
    P --> F["Files"]
    P --> S["System Monitoring"]
    P --> A["ASP.NET Core API"]
    A --> DB["SQLite"]
    A --> SSD["2 TB SSD"]
    P --> N["Nginx"]
    N --> T["Cloudflare Tunnel"]
    T --> I["Internet (Optional)"]

    subgraph Local_Network["Offline-First Home Network"]
        P
        A
        DB
        SSD
        N
    end
```

### Chapter 01 — Platform Identity

```mermaid
flowchart LR
    subgraph Philosophy["Platform Identity"]
        direction TB
        A["Personal Infrastructure Platform"]
        B["(not a media server)"]
    end

    C["Capabilities"] --> A
    D["Media Management"] --> C
    E["File Management"] --> C
    F["System Monitoring"] --> C
    G["Future Services"] --> C

    A --> H["Offline First"]
    A --> I["Privacy First"]
    A --> J["Resource Efficient"]
```

### Chapter 02 — Actors

```mermaid
flowchart LR
    subgraph Actors["Actors"]
        A["Admin/Owner"]
        B["Family Member"]
        C["Guest (future)"]
    end

    subgraph System["BhusalHub"]
        S["Platform"]
    end

    A --> S
    B --> S
    C -.->|"V2+" | S

    A -.->|"manages" | B
```

### Chapter 03 — NFR Hierarchy

```mermaid
flowchart TD
    NFR01["NFR-01: Performance"] --> T1["< 2s gallery load"]
    NFR01 --> T2["< 500ms API p95"]
    NFR02["NFR-02: Availability"] --> T3["99% uptime"]
    NFR02 --> T4["120s recovery"]
    NFR03["NFR-03: Resources"] --> T5["< 1.4 GB RAM"]
    NFR03 --> T6["< 100 MB DB"]
    NFR04["NFR-04: Security"] --> T7["TLS 1.2+"]
    NFR04 --> T8["Zero inbound ports"]
    NFR05["NFR-05: Reliability"] --> T9["Survives power loss"]
    NFR05 --> T10["Graceful degradation"]
    NFR06["NFR-06: Scalability"] --> T11["3 concurrent users"]
    NFR06 --> T12["100K files"]
```

### Chapter 04 — Decision Flow

```mermaid
flowchart TD
    Q1["Does it work offline?"] -->|"No"| R1["REJECT"]
    Q1 -->|"Yes"| Q2["Does it fit in 2GB RAM?"]
    Q2 -->|"No"| R2["Find lighter alternative"]
    Q2 -->|"Yes"| Q3["Is it the simplest option?"]
    Q3 -->|"No"| R3["Can we simplify?"]
    Q3 -->|"Yes"| Q4["Can it recover automatically?"]
    Q4 -->|"No"| R4["Add recovery mechanism"]
    Q4 -->|"Yes"| A1["ACCEPT"]
```

### Chapter 07 — System Context

```mermaid
flowchart TD
    U["User\n(Person)"] -->|"Browses media, uploads files\nvia HTTPS"| BH["BhusalHub\n[Software System]"]
    U -->|"Manages system\nvia HTTPS"| BH

    BH -->|"Reads/writes media files"| SSD["Local SSD Storage\n[Infrastructure]"]
    BH -->|"Reads/writes metadata"| DB["SQLite Database\n[Infrastructure]"]

    BH -->|"Optional: remote tunnel\n(cloudflared)"| CT["Cloudflare Tunnel\n[External System]"]
    CT -->|"Encrypted tunnel"| CF["Cloudflare Edge\n[External System]"]
    CF -->|"HTTPS"| RU["Remote User\n(Person)"]

    BH -->|"Optional: image updates"| WT["Watchtower\n[External System]"]
    WT -->|"Pulls images"| DH["Docker Hub\n[External System]"]

    subgraph Home_Network["Home Network"]
        BH
        SSD
        DB
        U
    end
```

### Chapter 08 — Container Diagram

```mermaid
flowchart TD
    U["User\n[Browser]"] -->|"HTTPS"| N["Nginx\n[Reverse Proxy &\nStatic File Server]"]

    N -->|"/api/*"| API["ASP.NET Core API\n[Web API Container]"]
    N -->|"/uploads/*"| FS["File System\n[Media Storage on SSD]"]
    N -->|"/* (static)"| UI["React SPA\n[Static Files]"]
    N -->|"/thumbnails/*"| TH["Thumbnails\n[Generated Images on SSD]"]

    API -->|"Reads/Writes"| DB["SQLite\n[Metadata Database]"]
    API -->|"Reads/Writes"| FS
    API -->|"Generates"| TH
    API -->|"Reads"| UI

    subgraph Docker_Host["Raspberry Pi (Docker Host)"]
        N
        API
        DB
        FS
        TH
        UI
    end

    N -->|"Optional"| CF["Cloudflare Tunnel\n[cloudflared container]"]
```

### Chapter 09 — Deployment Topology

```mermaid
flowchart TD
    HW["Hardware Layer"] -->|"USB 3.0"| SSD["2 TB USB SSD\n(ext4, LUKS optional)"]
    HW -->|"boots from"| SD["32 GB SD Card\n(Raspberry Pi OS Lite 64-bit)"]
    HW -->|"power"| PS["5V/3A USB-C PSU"]

    SD -->|"runs"| OS["Raspberry Pi OS Lite\n(kernel 6.x, no desktop)"]
    OS -->|"installs"| DE["Docker Engine\n(CE, managed by systemd)"]
    OS -->|"installs"| DC["Docker Compose\n(plugin)"]

    DE -->|"orchestrates"| CS["Container Stack"]
    CS --> N["Nginx Container"]
    CS --> A["API Container"]
    CS --> CF["cloudflared Container\n(optional)"]
    CS --> WT["Watchtower Container\n(optional)"]

    SSD -->|"mounted at"| MP["/mnt/ssd"]
    MP --> D1["/mnt/ssd/media"]
    MP --> D2["/mnt/ssd/thumbnails"]
    MP --> D3["/mnt/ssd/db"]
    MP --> D4["/mnt/ssd/config"]

    subgraph Physical_RPi["Raspberry Pi 4B (2GB)"]
        HW
        SD
        PS
    end
```

### Chapter 10 — Network Topology

```mermaid
flowchart TD
    subgraph WAN["Internet"]
        CF["Cloudflare Edge"]
    end

    subgraph Router["Home Router (192.168.1.1)"]
        DHCP["DHCP Server"]
        DNS["DNS Forwarder\n(split DNS)"]
        NAT["NAT / Firewall"]
    end

    subgraph LAN["Home LAN (192.168.1.0/24)"]
        RPI["Raspberry Pi\neth0: 192.168.1.100"]
        C1["Client Laptop\n192.168.1.x"]
        C2["Phone\n192.168.1.x"]
        C3["TV\n192.168.1.x"]
    end

    subgraph Docker["BhusalHub Docker Network (172.18.0.0/16)"]
        N["Nginx\n172.18.0.2"]
        API["API\n172.18.0.3"]
        CFd["cloudflared\n172.18.0.4"]
    end

    C1 -->|"home.bharatbhusal.com\n→ 192.168.1.100:443"| RPI
    C2 --> RPI
    C3 --> RPI

    RPI --> N
    N --> API

    CFd -->|"outbound tunnel\n(tcp/443)"| CF
    CF -->|"home.bharatbhusal.com"| RU["Remote User"]

    Router -->|"internet"| WAN
    RPI -->|"eth0"| Router
```

### Chapter 11 — Service Dependencies

```mermaid
flowchart TD
    subgraph Services["BhusalHub Services"]
        N["Nginx\nReverse Proxy"]
        API["API\nASP.NET Core"]
        CF["cloudflared\nTunnel Client"]
        WT["Watchtower\nAuto-Updater"]
    end

    subgraph Storage["Storage"]
        SQL["SQLite\n(File-based)"]
        FS["Filesystem\n(SSD)"]
    end

    subgraph External["External"]
        DH["Docker Hub"]
        CFE["Cloudflare Edge"]
    end

    U["User"] -->|"HTTPS"| N
    RU["Remote User"] -->|"HTTPS"| CFE
    CFE -->|"tunnel"| CF
    CF -->|"HTTP"| N
    N -->|"HTTP"| API
    API -->|"reads/writes"| SQL
    API -->|"reads/writes"| FS
    N -->|"reads"| FS
    WT -->|"pulls"| DH
    WT -->|"restarts"| API

    API -.->|"generates"| TH["Thumbnails (on FS)"]
```

### Chapter 12 — Docker Stack

```mermaid
flowchart TD
    subgraph Compose["docker-compose.yml"]
        direction TB
        N["bh-nginx\nnginx:alpine\nports: 443, 80"]
        API["bh-api\nbhusalhub/api:latest\nno ports exposed"]
        CF["bh-tunnel\ncloudflare/cloudflared\nno ports"]
        WT["bh-watchtower\ncontainrrr/watchtower\nno ports, socket mounted"]
    end

    subgraph Volumes["Shared Volumes"]
        MEDIA["bh_media\n/data/media"]
        THUMBS["bh_thumbnails\n/data/thumbnails"]
        DB["bh_db\n/data/db"]
        CONFIG["bh_config\n/data/config"]
    end

    subgraph Network["bh-network (bridge)"]
        N
        API
        CF
    end

    N -->|"volume"| MEDIA
    N -->|"volume"| THUMBS
    API -->|"volume"| MEDIA
    API -->|"volume"| THUMBS
    API -->|"volume"| DB
    API -->|"volume"| CONFIG
    CF -->|"volume"| CONFIG
```

*For the full list of all diagrams, see the individual handbook chapters.*
