#!/bin/bash
set -euo pipefail

echo "Starting BhusalHub development environment..."

echo "Starting backend..."
cd src/api
dotnet run --project BhusalHub.Api/BhusalHub.Api.csproj &
BACKEND_PID=$!

echo "Starting frontend..."
cd ../web
npm install --silent
npm run dev &
FRONTEND_PID=$!

echo "Backend PID: $BACKEND_PID"
echo "Frontend PID: $FRONTEND_PID"
echo ""
echo "Frontend: http://localhost:5173"
echo "API:      http://localhost:8080/api/health"

trap "kill $BACKEND_PID $FRONTEND_PID 2>/dev/null" EXIT
wait
