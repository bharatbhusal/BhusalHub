#!/bin/bash
set -euo pipefail

BACKUP_DIR="/mnt/ssd/backups/db"
DB_PATH="/mnt/ssd/db/bhusalhub.db"
DATE=$(date +%Y-%m-%d)

mkdir -p "$BACKUP_DIR"

if [[ -f "$DB_PATH" ]]; then
    sqlite3 "$DB_PATH" ".backup '$BACKUP_DIR/bhusalhub-$DATE.db'"
    find "$BACKUP_DIR" -name "bhusalhub-*.db" -mtime +30 -delete
    echo "Backup created: $BACKUP_DIR/bhusalhub-$DATE.db"
else
    echo "Database not found at $DB_PATH"
    exit 1
fi
