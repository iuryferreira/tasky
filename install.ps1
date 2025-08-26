# ======================================
# Instalador Tasky CLI (Windows)
# ======================================

param(
    [string]$Version = "Release",
    [string]$Runtime = "win-x64"
)

Write-Host "`n🚀 Iniciando instalação do Tasky CLI..." -ForegroundColor Cyan

# Caminhos
$projectPath = "src/Tasky.Cli"
$installDir  = "C:\Program Files\Tasky"
$configDir   = Join-Path $env:LOCALAPPDATA "Tasky"

try {
    # 1. Publicar build self-contained (silencioso)
    Write-Host "🔨 Compilando aplicação..." -ForegroundColor DarkGray
    dotnet publish $projectPath -c $Version -r $Runtime --self-contained true `
        -p:PublishSingleFile=true -p:PublishTrimmed=true -p:OutputName=tasky `
        | Out-Null

    # Detectar pasta do publish automaticamente (pega primeiro net* encontrado)
    $publishDir = Get-ChildItem "$projectPath\bin\$Version" -Directory |
                  Where-Object { $_.Name -like "net*" } |
                  Select-Object -First 1 |
                  ForEach-Object { "$($_.FullName)\$Runtime\publish" }

    if (-Not (Test-Path "$publishDir\tasky.exe")) {
        throw "Não foi possível encontrar o executável em $publishDir"
    }

    # 2. Criar pasta de instalação
    if (-Not (Test-Path $installDir)) {
        Write-Host "📂 Criando pasta de instalação em $installDir"
        New-Item -ItemType Directory -Force -Path $installDir | Out-Null
    }

    # 3. Copiar executável
    Write-Host "📦 Instalando executável em $installDir"
    Copy-Item "$publishDir\tasky.exe" $installDir -Force

    # 4. Criar diretório de configuração do usuário
    if (-Not (Test-Path $configDir)) {
        Write-Host "⚙️ Criando pasta de configuração em $configDir"
        New-Item -ItemType Directory -Force -Path $configDir | Out-Null
    }

    # 5. Copiar appsettings.json padrão
    $appsettingsSrc = Join-Path $publishDir "appsettings.json"
    if (-Not (Test-Path $appsettingsSrc)) {
        # se não estiver no publish, tenta na pasta acima (bin\Release\netX.Y\)
        $appsettingsSrc = Join-Path (Split-Path $publishDir -Parent) "appsettings.json"
    }

    $appsettingsDst = Join-Path $configDir "appsettings.json"

    if (-Not (Test-Path $appsettingsDst)) {
        if (Test-Path $appsettingsSrc) {
            Write-Host "📑 Copiando configuração inicial..."
            Copy-Item $appsettingsSrc $appsettingsDst -Force
        }
        else {
            Write-Host "⚠️ Nenhum appsettings.json encontrado (nem em publish nem no bin). Criando vazio..."
            '{}' | Out-File $appsettingsDst -Encoding utf8
        }
    }

    # 6. Adicionar ao PATH do usuário (se não existir)
    $taskyPath = "$installDir"
    $userPath = [Environment]::GetEnvironmentVariable("Path", "User")

    if ($userPath -notlike "*$taskyPath*") {
        Write-Host "🔗 Adicionando Tasky ao PATH do usuário..."
        [Environment]::SetEnvironmentVariable("Path", "$userPath;$taskyPath", "User")
    }

    Write-Host "`n✅ Tasky instalado com sucesso!" -ForegroundColor Green
    Write-Host "👉 Feche e reabra o terminal, depois rode: " -NoNewline
    Write-Host "tasky" -ForegroundColor Yellow
}
catch {
    Write-Host "`n❌ Erro durante a instalação: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}
