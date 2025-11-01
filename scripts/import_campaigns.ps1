param(
    [string]$BaseUrl = "https://localhost:7032",
    [string]$JsonPath = "c:\\Users\\Rowan\\OneDrive\\Desktop\\collage\\depi\\graduationProject\\project\\Athar\\scrapers\\megakheir\\data\\megakheir_campaigns_23.json",
    [string]$User = "admin@athar.local",
    [string]$Password = "Admin#123",
    [switch]$UseMinimalBody
)

Write-Host "Logging in to $BaseUrl ..."
$loginBody = @{ UserNameOrEmail = $User; Password = $Password } | ConvertTo-Json
$login = Invoke-RestMethod -Uri "$BaseUrl/api/Account/Login" -Method Post -ContentType "application/json" -Body $loginBody
if (-not $login -or -not $login.token) {
    throw "Login failed"
}
$jwt = $login.token
Write-Host "Login OK. Importing campaigns from $JsonPath ..."

if ($UseMinimalBody) {
    $json = '[{"title":"t","description":"d","image_url":"http://example.com/x.jpg"}]'
    try {
        $import = Invoke-RestMethod -Uri "$BaseUrl/api/Campaign/import" -Method Post -Headers @{ Authorization = "Bearer $jwt" } -ContentType "application/json" -Body $json -ErrorAction Stop
    } catch {
        $respContent = $null; $statusCode = $null
        if ($_.Exception.Response) {
            $statusCode = [int]$_.Exception.Response.StatusCode
            $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
            $respContent = $reader.ReadToEnd(); $reader.Dispose()
        }
        Write-Host "Import failed with status: $statusCode" -ForegroundColor Red
        if ($respContent) { Write-Host $respContent }
        throw
    }
} else {
    if (-not (Test-Path -Path $JsonPath)) {
        throw "JSON file not found: $JsonPath"
    }
    try {
        # Send file bytes directly to avoid any encoding/body formatting issues
        $import = Invoke-RestMethod -Uri "$BaseUrl/api/Campaign/import" -Method Post -Headers @{ Authorization = "Bearer $jwt" } -ContentType "application/json" -InFile $JsonPath -ErrorAction Stop
    } catch {
        $respContent = $null; $statusCode = $null
        if ($_.Exception.Response) {
            $statusCode = [int]$_.Exception.Response.StatusCode
            $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
            $respContent = $reader.ReadToEnd(); $reader.Dispose()
        }
        Write-Host "Import failed with status: $statusCode" -ForegroundColor Red
        if ($respContent) { Write-Host $respContent }
        throw
    }
}
Write-Host "Import summary:" -ForegroundColor Cyan
$import | ConvertTo-Json -Depth 6

Write-Host "\nChecking campaigns list (page 1) ..."
$all = Invoke-RestMethod -Uri "$BaseUrl/api/Campaign/GetAll?page=1&pageSize=12" -Method Get
"Total campaigns: " + $all.total
"Returned: " + ($all.items | Measure-Object | Select-Object -ExpandProperty Count)
$all.items | Select-Object -First 5 id, title, charityName | Format-Table | Out-String | Write-Host

Write-Host "\nFetching per-charity campaign counts ..."
$counts = Invoke-RestMethod -Uri "$BaseUrl/api/charities/campaign-counts" -Method Get
$counts | Select-Object -First 10 | Format-Table CharityId, CharityName, CampaignsCount | Out-String | Write-Host

Write-Host "\nFetching campaigns summary (JSON) ..."
$summary = Invoke-RestMethod -Uri "$BaseUrl/api/charities/campaigns-summary?includeCampaigns=true&format=json" -Method Get
$summary | Select-Object -First 5 CharityName, CampaignsCount | Format-Table | Out-String | Write-Host

Write-Host "\nDone." -ForegroundColor Green
