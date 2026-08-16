#!/usr/bin/env bash
# Build LukesWall for macOS / .NET 8 (MonoGame DesktopGL).
# Usage: ./build-mac.sh [-c Release]  (any extra args are passed to dotnet build)
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
LUKES_WALL_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"
MAC_DIR="$LUKES_WALL_ROOT/LukesWall.Mac"

cd "$MAC_DIR"

# Local dotnet tools (mgcb) resolve via ~/.dotnet/toolResolverCache. If NUGET_PACKAGES
# points at an ephemeral directory (e.g. some IDE sandboxes), restored tools and the
# cache can disagree and `dotnet mgcb` fails even after `dotnet tool restore`.
if [[ "${NUGET_PACKAGES:-}" == *cursor-sandbox-cache* ]]; then
  unset NUGET_PACKAGES
fi

dotnet tool restore
dotnet build LukesWall.Mac.sln "$@"
