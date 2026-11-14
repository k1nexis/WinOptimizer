$ErrorActionPreference = "Stop"

$root = Split-Path -Parent $MyInvocation.MyCommand.Path
$project = Join-Path $root "WinOptimizer.csproj"
$releaseDir = Join-Path $root "bin\Release"
$distDir = Join-Path $root "dist"
$msbuild = Get-Command msbuild.exe -ErrorAction SilentlyContinue

if ($msbuild) {
    & $msbuild.Source $project /m /p:Configuration=Release '/p:Platform=Any CPU' /verbosity:minimal
} else {
    & dotnet build $project --configuration Release
}
if ($LASTEXITCODE -ne 0) { throw "Release build failed with exit code $LASTEXITCODE." }

New-Item -ItemType Directory -Path $distDir -Force | Out-Null
Copy-Item -LiteralPath (Join-Path $releaseDir "WinOptimizer.exe") -Destination (Join-Path $distDir "WinOptimizer.exe") -Force
Copy-Item -LiteralPath (Join-Path $root "README.md") -Destination (Join-Path $distDir "README.md") -Force
Copy-Item -LiteralPath (Join-Path $root "PUBLISHING.md") -Destination (Join-Path $distDir "PUBLISHING.md") -Force
$hash = (Get-FileHash -LiteralPath (Join-Path $distDir "WinOptimizer.exe") -Algorithm SHA256).Hash.ToLowerInvariant()
Set-Content -LiteralPath (Join-Path $distDir "WinOptimizer.exe.sha256") -Value "$hash  WinOptimizer.exe" -Encoding ascii
$archive = Join-Path $distDir "WinOptimizer.zip"
Compress-Archive -Path (Join-Path $distDir "WinOptimizer.exe"),(Join-Path $distDir "README.md"),(Join-Path $distDir "PUBLISHING.md"),(Join-Path $distDir "WinOptimizer.exe.sha256") -DestinationPath $archive -Force
Write-Host "Release files created:"
Write-Host (Join-Path $distDir "WinOptimizer.exe")
Write-Host $archive
