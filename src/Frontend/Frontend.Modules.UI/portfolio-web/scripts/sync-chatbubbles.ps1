param(
  [string]$Configuration = "Release"
)

$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$modulesRoot = Resolve-Path (Join-Path $here "..\..")
$wasmCsproj = Join-Path $modulesRoot "Portfolio.ChatBubbles.Wasm\Portfolio.ChatBubbles.Wasm.csproj"
$outDir = Join-Path $modulesRoot "out-chatbubbles"
$reactPublic = Resolve-Path (Join-Path $here "..\public")

if (-not (Test-Path $wasmCsproj)) { throw "Projeto Blazor não encontrado: $wasmCsproj" }

dotnet publish $wasmCsproj -c $Configuration -o $outDir

$srcFramework = Join-Path $outDir "wwwroot\_framework"
$srcContent = Join-Path $outDir "wwwroot\_content"
$dstFramework = Join-Path $reactPublic "_framework"
$dstContent = Join-Path $reactPublic "_content"

if (-not (Test-Path $srcFramework)) { throw "Fonte _framework não encontrada: $srcFramework" }
if (-not (Test-Path $srcContent)) { throw "Fonte _content não encontrada: $srcContent" }

if (Test-Path $dstFramework) { Remove-Item $dstFramework -Recurse -Force }
if (Test-Path $dstContent)  { Remove-Item $dstContent  -Recurse -Force }

New-Item $dstFramework -ItemType Directory | Out-Null
New-Item $dstContent  -ItemType Directory | Out-Null

Copy-Item "$srcFramework\*" $dstFramework -Recurse -Force
Copy-Item "$srcContent\*"  $dstContent  -Recurse -Force

$loaderHashed = Get-ChildItem $dstFramework -Filter "blazor.webassembly*.js" | Where-Object { $_.Name -ne "blazor.webassembly.js" } | Sort-Object LastWriteTime -Descending | Select-Object -First 1
if ($loaderHashed) { Copy-Item $loaderHashed.FullName (Join-Path $dstFramework "blazor.webassembly.js") -Force }

Write-Host "Blazor assets sincronizados para $reactPublic"
