#!/usr/bin/env bash
# Run the Mac DesktopGL build of LukesWall.
# Optional: first args can be -c Release (or pass through to dotnet run after --).
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
LUKES_WALL_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"
PROJECT="$LUKES_WALL_ROOT/LukesWall.Mac/LukesWall.Mac.csproj"

dotnet run --project "$PROJECT" "$@"
