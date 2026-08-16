# Build LukesWall for Windows (legacy .NET Framework 4.5 + MonoGame WinDX).
# Requires MSBuild from Visual Studio or Build Tools. Run on Windows.
# Usage: .\build-win.ps1 [-Configuration Debug|Release] [-Platform x86]

param(
    [string]$Configuration = "Release",
    [string]$Platform = "x86"
)

$ErrorActionPreference = "Stop"
$gameRoot = Split-Path -Parent $PSScriptRoot
$sln = Join-Path $gameRoot "LukesWall.sln"

if (-not (Test-Path $sln)) {
    Write-Error "Solution not found: $sln"
}

$msbuild = $null
foreach ($c in @(
    "${env:ProgramFiles}\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe",
    "${env:ProgramFiles}\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe",
    "${env:ProgramFiles}\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe",
    "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe",
    "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe"
)) {
    if ($c -and (Test-Path $c)) {
        $msbuild = $c
        break
    }
}

if (-not $msbuild) {
    $msbuild = Get-Command msbuild -ErrorAction SilentlyContinue | Select-Object -ExpandProperty Source
}

if (-not $msbuild) {
    Write-Error "MSBuild not found. Install Visual Studio or Build Tools, or add MSBuild to PATH."
}

& $msbuild $sln /restore /p:Configuration=$Configuration /p:Platform=$Platform /verbosity:minimal
