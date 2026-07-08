#!/bin/bash
echo "Stopping BhusalHub development processes..."
pkill -f "dotnet run" 2>/dev/null || true
pkill -f "vite" 2>/dev/null || true
echo "Stopped."
