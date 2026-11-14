$ErrorActionPreference = "Stop"

$root = Split-Path -Parent $MyInvocation.MyCommand.Path
$project = Join-Path $root "WindowsOptimizerForAll.csproj"
$releaseDir = Join-Path $root "bin\Release"
$distDir = Join-Path $root "dist"
$msbuild = Get-Command msbuild.exe -ErrorAction SilentlyContinue

if ($msbuild) {
    # The project declares its configuration as Release|AnyCPU. Passing "Any CPU"
    # prevents MSBuild from matching that property group and leaves OutputPath empty.
    & $msbuild.Source $project /m /p:Configuration=Release /p:Platform=AnyCPU /verbosity:minimal
} else {
    & dotnet build $project --configuration Release
}
if ($LASTEXITCODE -ne 0) { throw "Release build failed with exit code $LASTEXITCODE." }

New-Item -ItemType Directory -Path $distDir -Force | Out-Null
New-Item -ItemType Directory -Path (Join-Path $distDir "assets") -Force | Out-Null
Copy-Item -LiteralPath (Join-Path $releaseDir "WindowsOptimizerForAll.exe") -Destination (Join-Path $distDir "WindowsOptimizerForAll.exe") -Force
Copy-Item -LiteralPath (Join-Path $root "README.md") -Destination (Join-Path $distDir "README.md") -Force
Copy-Item -LiteralPath (Join-Path $root "PUBLISHING.md") -Destination (Join-Path $distDir "PUBLISHING.md") -Force
Copy-Item -LiteralPath (Join-Path $root "assets\windows-optimizer-for-all.svg") -Destination (Join-Path $distDir "assets\windows-optimizer-for-all.svg") -Force
Copy-Item -LiteralPath (Join-Path $root "assets\windows-optimizer-for-all.ico") -Destination (Join-Path $distDir "assets\windows-optimizer-for-all.ico") -Force
$hash = (Get-FileHash -LiteralPath (Join-Path $distDir "WindowsOptimizerForAll.exe") -Algorithm SHA256).Hash.ToLowerInvariant()
Set-Content -LiteralPath (Join-Path $distDir "WindowsOptimizerForAll.exe.sha256") -Value "$hash  WindowsOptimizerForAll.exe" -Encoding ascii
$archive = Join-Path $distDir "Windows-Optimizer-for-All.zip"
Compress-Archive -Path (Join-Path $distDir "WindowsOptimizerForAll.exe"),(Join-Path $distDir "README.md"),(Join-Path $distDir "PUBLISHING.md"),(Join-Path $distDir "WindowsOptimizerForAll.exe.sha256"),(Join-Path $distDir "assets") -DestinationPath $archive -Force
Write-Host "Release files created:"
Write-Host (Join-Path $distDir "WindowsOptimizerForAll.exe")
Write-Host $archive
