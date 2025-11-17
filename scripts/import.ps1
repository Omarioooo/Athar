param(
  [string]$BaseUrl = "http://localhost:5290",
  [string]$AdminEmail = "admin@athar.local",
  [string]$AdminPassword = "Admin#123",
  # Default to full dataset
  [string]$CharitiesJson = "Athar\scrapers\megakheir\data\charities101.json",
  # Default campaigns dataset
  [string]$CampaignsJson = "Athar\scrapers\megakheir\data\campagins23.json",
  [int]$CharityBatchSize = 25,
  [int]$CampaignBatchSize = 20
)

$ErrorActionPreference = 'Stop'

function Wait-ServerReady([string]$url, [int]$timeoutSeconds = 60) {
  $sw = [Diagnostics.Stopwatch]::StartNew()
  while ($sw.Elapsed.TotalSeconds -lt $timeoutSeconds) {
    try {
      # Lightweight health endpoint
      Invoke-RestMethod -Method Get -Uri "$url/health" -Headers @{ Accept = 'application/json' } | Out-Null
      return $true
    } catch { Start-Sleep -Seconds 1 }
  }
  return $false
}

if (-not (Wait-ServerReady -url $BaseUrl -timeoutSeconds 60)) {
  Write-Error "API not reachable at $BaseUrl. Start the API first (dotnet run)."
  exit 1
}

# 1) Register Admin (idempotent)
try {
  $adminBody = @{ FirstName='Admin'; LastName='User'; Email=$AdminEmail; Password=$AdminPassword; Country='EG'; City='Cairo' } | ConvertTo-Json
  $regRes = Invoke-RestMethod -Method Post -Uri "$BaseUrl/api/Account/AdminRegisterJson" -Body $adminBody -ContentType 'application/json'
  Write-Host "Admin registration:" ($regRes | ConvertTo-Json -Depth 5)
} catch {
  Write-Warning "Admin registration may have failed or already exists: $($_.Exception.Message)"
}

# 2) Login
$loginBody = @{ UserNameOrEmail = $AdminEmail; Password = $AdminPassword } | ConvertTo-Json
$loginRes = Invoke-RestMethod -Method Post -Uri "$BaseUrl/api/Account/Login" -Body $loginBody -ContentType 'application/json'
$TOKEN = $loginRes.token
if (-not $TOKEN) { Write-Error "Login did not return token."; exit 1 }
Write-Host "Got JWT token (truncated):" ($TOKEN.Substring(0,24) + '...')

# 3) Import charities
if (Test-Path $CharitiesJson) {
  Write-Host "Importing charities from $CharitiesJson in batches of $CharityBatchSize ..."
  $raw = Get-Content -Raw -Path $CharitiesJson
  $allItems = $raw | ConvertFrom-Json
  if (-not $allItems) { Write-Error "Parsed charities JSON is empty"; exit 1 }
  $total = $allItems.Count
  $index = 0
  $stopwatch = [System.Diagnostics.Stopwatch]::StartNew()
  while ($index -lt $total) {
    $batch = $allItems[$index..([Math]::Min($index+$CharityBatchSize-1,$total-1))]
    $batchJson = $batch | ConvertTo-Json -Depth 6
    try {
      $res = Invoke-RestMethod -Method Post -Uri "$BaseUrl/api/Charities/import" -Body $batchJson -ContentType 'application/json' -Headers @{ Authorization = "Bearer $TOKEN" }
      Write-Host ("[Charities] Imported batch {0}-{1} | elapsed {2:n1}s | response imported={3}" -f $index, ($index+$batch.Count-1), $stopwatch.Elapsed.TotalSeconds, $res.imported)
    } catch {
      Write-Warning ("[Charities] Batch {0}-{1} failed: {2}" -f $index, ($index+$batch.Count-1), $_.Exception.Message)
    }
    $index += $CharityBatchSize
  }
  $stopwatch.Stop()
  Write-Host "Finished charities import. Total items processed: $total in $($stopwatch.Elapsed.TotalSeconds.ToString('n1'))s"
} else {
  Write-Warning "Charities JSON not found: $CharitiesJson"
}

# 4) Import campaigns (optional)
if ($CampaignsJson -and (Test-Path $CampaignsJson)) {
  Write-Host "Importing campaigns from $CampaignsJson in batches of $CampaignBatchSize ..."
  $rawC = Get-Content -Raw -Path $CampaignsJson
  $allCampaigns = $rawC | ConvertFrom-Json
  if ($allCampaigns) {
    $ctotal = $allCampaigns.Count
    $cIndex = 0
    $cStopwatch = [System.Diagnostics.Stopwatch]::StartNew()
    while ($cIndex -lt $ctotal) {
      $cbatch = $allCampaigns[$cIndex..([Math]::Min($cIndex+$CampaignBatchSize-1,$ctotal-1))]
      $cbatchJson = $cbatch | ConvertTo-Json -Depth 6
      try {
        $cres = Invoke-RestMethod -Method Post -Uri "$BaseUrl/api/Campaign/import" -Body $cbatchJson -ContentType 'application/json' -Headers @{ Authorization = "Bearer $TOKEN" }
        Write-Host ("[Campaigns] Imported batch {0}-{1} | elapsed {2:n1}s | response imported={3}" -f $cIndex, ($cIndex+$cbatch.Count-1), $cStopwatch.Elapsed.TotalSeconds, $cres.imported)
      } catch {
        Write-Warning ("[Campaigns] Batch {0}-{1} failed: {2}" -f $cIndex, ($cIndex+$cbatch.Count-1), $_.Exception.Message)
      }
      $cIndex += $CampaignBatchSize
    }
    $cStopwatch.Stop()
    Write-Host "Finished campaigns import. Total items processed: $ctotal in $($cStopwatch.Elapsed.TotalSeconds.ToString('n1'))s"
  } else {
    Write-Warning "Parsed campaigns JSON is empty"
  }
} else {
  if ($CampaignsJson) { Write-Warning "Campaigns JSON not found: $CampaignsJson" } else { Write-Host "Skipping campaigns import (no JSON specified)." }
}

# 5) Sanity checks
Write-Host "Fetching charities page 1..."
$pg1 = Invoke-RestMethod -Method Get -Uri "$BaseUrl/api/Charities?page=1&pageSize=12" -Headers @{ Accept = 'application/json' }
Write-Host ("Charities count total=" + $pg1.total + ", page items=" + ($pg1.items | Measure-Object | Select-Object -ExpandProperty Count))
