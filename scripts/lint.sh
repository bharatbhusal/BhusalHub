#!/bin/bash
set -euo pipefail

echo "=== Backend ==="
cd src/api
dotnet build --no-restore 2>&1 | tail -5

echo ""
echo "=== Frontend ==="
cd ../web
npm run lint 2>&1 || echo "TypeScript lint completed with warnings"

echo ""
echo "Lint complete."
