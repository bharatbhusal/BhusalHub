# Appendix C — Database Schema

**Status:** Draft  
**Last Updated:** 2026-07-08  
**Owner:** Bharat Bhusal  
**References:** [14 - Database Architecture](../part-ii-architecture/14-database-architecture.md)

---

## Purpose

Provide the complete SQLite database schema for reference.

## Scope

All tables, indexes, and constraints. This is the one-to-one mapping of the ER diagram in [Chapter 14](../part-ii-architecture/14-database-architecture.md).

## Schema DDL

### Users

```sql
CREATE TABLE IF NOT EXISTS User (
    id TEXT PRIMARY KEY,
    username TEXT NOT NULL UNIQUE,
    password_hash TEXT NOT NULL,
    display_name TEXT NOT NULL,
    role TEXT NOT NULL DEFAULT 'user' CHECK(role IN ('admin', 'user')),
    created_at TEXT NOT NULL DEFAULT (datetime('now')),
    updated_at TEXT NOT NULL DEFAULT (datetime('now'))
);

CREATE INDEX idx_user_username ON User(username);
```

### Sessions

```sql
CREATE TABLE IF NOT EXISTS Session (
    id TEXT PRIMARY KEY,
    user_id TEXT NOT NULL REFERENCES User(id) ON DELETE CASCADE,
    token_hash TEXT NOT NULL UNIQUE,
    expires_at TEXT NOT NULL,
    created_at TEXT NOT NULL DEFAULT (datetime('now'))
);

CREATE INDEX idx_session_token ON Session(token_hash);
CREATE INDEX idx_session_expires ON Session(expires_at);
CREATE INDEX idx_session_user ON Session(user_id);
```

### Media

```sql
CREATE TABLE IF NOT EXISTS Media (
    id TEXT PRIMARY KEY,
    filename_original TEXT NOT NULL,
    filename_stored TEXT NOT NULL,
    file_path TEXT NOT NULL,
    media_type TEXT NOT NULL CHECK(media_type IN ('photo', 'video', 'document', 'other')),
    mime_type TEXT NOT NULL,
    file_size INTEGER NOT NULL,
    width INTEGER,
    height INTEGER,
    duration INTEGER,
    thumbnail_path TEXT,
    thumbnail_status TEXT NOT NULL DEFAULT 'completed' CHECK(thumbnail_status IN ('pending', 'completed', 'failed')),
    checksum TEXT NOT NULL,
    created_at TEXT NOT NULL DEFAULT (datetime('now')),
    updated_at TEXT NOT NULL DEFAULT (datetime('now')),
    uploader_id TEXT NOT NULL REFERENCES User(id)
);

CREATE INDEX idx_media_type ON Media(media_type);
CREATE INDEX idx_media_created ON Media(created_at DESC);
CREATE INDEX idx_media_uploader ON Media(uploader_id);
CREATE INDEX idx_media_checksum ON Media(checksum);
CREATE INDEX idx_media_thumbnail_status ON Media(thumbnail_status);
```

### Albums

```sql
CREATE TABLE IF NOT EXISTS Album (
    id TEXT PRIMARY KEY,
    name TEXT NOT NULL,
    description TEXT DEFAULT '',
    cover_media_id TEXT REFERENCES Media(id) ON DELETE SET NULL,
    owner_id TEXT NOT NULL REFERENCES User(id),
    created_at TEXT NOT NULL DEFAULT (datetime('now')),
    updated_at TEXT NOT NULL DEFAULT (datetime('now'))
);

CREATE INDEX idx_album_owner ON Album(owner_id);
```

### AlbumMedia

```sql
CREATE TABLE IF NOT EXISTS AlbumMedia (
    album_id TEXT NOT NULL REFERENCES Album(id) ON DELETE CASCADE,
    media_id TEXT NOT NULL REFERENCES Media(id) ON DELETE CASCADE,
    sort_order INTEGER NOT NULL DEFAULT 0,
    added_at TEXT NOT NULL DEFAULT (datetime('now')),
    PRIMARY KEY (album_id, media_id)
);

CREATE INDEX idx_album_media_album ON AlbumMedia(album_id);
CREATE INDEX idx_album_media_media ON AlbumMedia(media_id);
```

### Tags

```sql
CREATE TABLE IF NOT EXISTS Tag (
    id TEXT PRIMARY KEY,
    name TEXT NOT NULL UNIQUE
);

CREATE INDEX idx_tag_name ON Tag(name);
```

### MediaTag

```sql
CREATE TABLE IF NOT EXISTS MediaTag (
    media_id TEXT NOT NULL REFERENCES Media(id) ON DELETE CASCADE,
    tag_id TEXT NOT NULL REFERENCES Tag(id) ON DELETE CASCADE,
    PRIMARY KEY (media_id, tag_id)
);

CREATE INDEX idx_media_tag_tag ON MediaTag(tag_id);
```

### Migrations

```sql
CREATE TABLE IF NOT EXISTS __Migrations (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT NOT NULL UNIQUE,
    applied_at TEXT NOT NULL DEFAULT (datetime('now'))
);
```

## Migration Example

```csharp
// Migration_001_InitialSchema.cs
public class Migration_001_InitialSchema : IMigration
{
    public string Name => "001_InitialSchema";
    public string Up => @"
        CREATE TABLE IF NOT EXISTS User ( ... );
        CREATE TABLE IF NOT EXISTS Session ( ... );
        CREATE TABLE IF NOT EXISTS Media ( ... );
        CREATE TABLE IF NOT EXISTS Album ( ... );
        CREATE TABLE IF NOT EXISTS AlbumMedia ( ... );
    ";
    public string Down => @"
        DROP TABLE IF EXISTS AlbumMedia;
        DROP TABLE IF EXISTS Album;
        DROP TABLE IF EXISTS Media;
        DROP TABLE IF EXISTS Session;
        DROP TABLE IF EXISTS User;
    ";
}
```

## Views (Future)

No views are defined for V1. If needed, they will be added here.

## Full-Text Search (Future)

When FTS5 is added for full-text search:

```sql
CREATE VIRTUAL TABLE IF NOT EXISTS MediaFts USING fts5(
    filename_original,
    content='Media',
    content_rowid='rowid'
);
```
