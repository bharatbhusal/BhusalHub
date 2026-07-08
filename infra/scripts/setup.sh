#!/bin/bash
set -euo pipefail

echo "=== BhusalHub Setup ==="

echo "Updating system..."
sudo apt update && sudo apt upgrade -y

echo "Installing Docker..."
curl -fsSL https://get.docker.com | sh
sudo usermod -aG docker "$USER"

echo "Installing Docker Compose plugin..."
sudo apt install -y docker-compose-plugin

echo "Mounting SSD..."
if ! mountpoint -q /mnt/ssd; then
    sudo mkdir -p /mnt/ssd
    echo "Please edit /etc/fstab to mount your SSD at /mnt/ssd"
    echo "Example: /dev/sda1 /mnt/ssd ext4 defaults,noatime,nodiratime 0 2"
fi

echo "Setup complete. Reboot or run 'newgrp docker' to apply group changes."
echo "After mounting the SSD, run: docker compose up -d"
