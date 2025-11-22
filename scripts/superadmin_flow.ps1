# SuperAdmin end-to-end flow
# Deletes charities 102 and 103, registers charity, approves it, creates campaign
# Arabic text is base64-encoded to avoid PowerShell 5.1 encoding issues

[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.SecurityProtocolType]::Tls12

$base = "http://localhost:5192"
$imgFolder = "C:\Users\Rowan\OneDrive\Desktop\collage\depi\graduationProject\project\imgs"

function Write-Section($title){ Write-Host "`n==== $title ====" -ForegroundColor Cyan }
function Fail($msg){ Write-Host "ERROR: $msg" -ForegroundColor Red; exit 1 }
function Login($user,$pass){
  $body = @{ UserNameOrEmail = $user; Password = $pass } | ConvertTo-Json -Compress
  try { Invoke-RestMethod -Uri "$base/api/Account/Login" -Method Post -ContentType "application/json" -Body $body -ErrorAction Stop }
  catch { Fail "Login failed for $user" }
}
function DecodeB64($b){ [System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String($b)) }

Write-Section "Login as SuperAdmin"
$super = Login "admin@athar.local" "Admin#123"
if (-not $super.token) { Fail "SuperAdmin token missing" }
$superHeaders = @{ Authorization = "Bearer $($super.token)" }
Write-Host "SuperAdmin token acquired." -ForegroundColor Green

Write-Section "Delete charities 102 and 103"
foreach ($id in 102,103){
  try {
    Invoke-RestMethod -Uri "$base/api/Charities/$id" -Method Delete -Headers $superHeaders -ErrorAction Stop
    Write-Host "Deleted charity $id" -ForegroundColor Yellow
  } catch {
    Write-Host "Skip delete $id" -ForegroundColor DarkGray
  }
}

Write-Section "Register charity"
Add-Type -AssemblyName System.Net.Http
$client = [System.Net.Http.HttpClient]::new()
$client.BaseAddress = [Uri]$base
$form = [System.Net.Http.MultipartFormDataContent]::new()

function Add-Field($name,$value){
  $sc = [System.Net.Http.StringContent]::new($value,[System.Text.UTF8Encoding]::UTF8,"text/plain")
  $sc.Headers.Remove("Content-Type")
  $sc.Headers.Add("Content-Disposition","form-data; name=`"$name`"")
  $form.Add($sc)
}

# Base64 encoded Arabic strings
$charName = DecodeB64 "2YTZhti52YXYsdmH2Kc="
$charEmail = "lnaamarha2026@charity.com"
$charPass = "Lu2810"
$charDesc = DecodeB64 "2YTZhti52YXYsdmH2Kcg2KfZhNiu2YrYsdmK2Kkg2YfZiiDZhdik2LPYs9ipINiu2YrYsdmK2Kkg2LrZitixINix2KjYrdmK2Kkg2KrYudmF2YQg2LnZhNmJINiq2YLYr9mK2YUg2KfZhNiv2LnZhSDYp9mE2KXZhtiz2KfZhtmKINmI2KfZhNin2KzYqtmF2KfYudmKINmE2YTZhdit2KrYp9is2YrZhiDZgdmKINmF2LXYsSDZhdmGINiu2YTYp9mEINio2LHYp9mF2Kwg2YXYqtmG2YjYudipINiq2LTZhdmEINin2YTYqti52YTZitmFINmI2KfZhNi12K3YqSDZiNin2YTYpdi62KfYq9ip"
$charCountry = DecodeB64 "2YXYtdix"
$charCity = DecodeB64 "2KfZhNin2LPZg9mG2K/YsdmK2Kk="

Add-Field "CharityName" $charName
Add-Field "Email" $charEmail
Add-Field "Password" $charPass
Add-Field "Description" $charDesc
Add-Field "Country" $charCountry
Add-Field "City" $charCity

$docPath = Join-Path $imgFolder "doc1.png"
$imgPath = Join-Path $imgFolder "charity_profile.jpg"
if (-not (Test-Path $docPath)) { Fail "Missing doc1.png" }
if (-not (Test-Path $imgPath)) { Fail "Missing charity_profile.jpg" }

$docStream = [System.IO.File]::OpenRead($docPath)
$docContent = [System.Net.Http.StreamContent]::new($docStream)
$docContent.Headers.ContentType = [System.Net.Http.Headers.MediaTypeHeaderValue]::Parse("image/png")
$docContent.Headers.Add("Content-Disposition",'form-data; name="VerificationDocument"; filename="doc1.png"')
$form.Add($docContent)

$imgStream = [System.IO.File]::OpenRead($imgPath)
$imgContent = [System.Net.Http.StreamContent]::new($imgStream)
$imgContent.Headers.ContentType = [System.Net.Http.Headers.MediaTypeHeaderValue]::Parse("image/jpeg")
$imgContent.Headers.Add("Content-Disposition",'form-data; name="ProfileImage"; filename="profile.jpg"')
$form.Add($imgContent)

$resp = $client.PostAsync("api/Account/CharityRegister", $form).Result
$docStream.Dispose(); $imgStream.Dispose()
if (-not $resp.IsSuccessStatusCode){ Fail "Charity registration failed" }
Write-Host "Charity registered." -ForegroundColor Green

Start-Sleep -Seconds 2
Write-Section "Lookup charity by name"
Add-Type -AssemblyName System.Web
$encodedName = [System.Web.HttpUtility]::UrlEncode($charName)
try { $charityObj = Invoke-RestMethod -Uri "$base/api/charities/byName/$encodedName" -Method Get -ErrorAction Stop }
catch { Fail "Lookup failed" }
$charityId = $charityObj.id
Write-Host "Resolved charity Id: $charityId" -ForegroundColor Green

Write-Section "Approve charity"
Invoke-RestMethod -Uri "$base/api/charities/$charityId/approve" -Method Post -Headers $superHeaders -ErrorAction Stop | Out-Null
Write-Host "Charity approved." -ForegroundColor Green

Write-Section "Login as CharityAdmin"
$charLogin = Login $charEmail $charPass
if (-not $charLogin.token){ Fail "CharityAdmin token missing" }
Write-Host "CharityAdmin token acquired." -ForegroundColor Green

Write-Section "Create campaign"
$campTitle = DecodeB64 "2YfZhti52YTZhdmH2YU="
$campDesc = DecodeB64 "2YLYp9mEINiq2LnYp9mE2Yk6CiDZiNmF2Kcg2KPZiNiq2YrYqtmFINmF2YYg2LTYpiDZgdmF2KrYp9i5INin2YTYrdmK2KfYqSDYp9mE2K/ZhtmK2Kcg2YjYstmK2YbYqtmH2Kcg2YjZhdinINi52YbYryDYp9mE2YTZhyDYrtmK2LEg2YjYo9io2YLZiSDYo9mB2YTYpyDYqti52YLZhNmI2YYgW9in2YTZgti12LU6IDYwXQrYp9mE2YfYr9mBINmH2Ygg2YbZiNixIDQ5INit2YLZitio2Kkg2YXYr9ix2LPZitipINmD2KfZhdmE2Kkg2KจYp9mE2KPYr9mI2KfYqiDYp9mE2K/Ysdin2LPZitipINi52YTZiSDYo9mH2KfZhNmK2YbYpyDZgdmKINi12LnZitivINin2YTYtNmG2K/Zitiv2YoKLSAyINmD2LHYp9iz2KkgKDEwMCDZiNix2YLYqSkKLSAxINmD2LHYp9iz2KkgKDYwINmI2LHZgtipKQotIDIg2YPYsdin2LPYqSAoNDAg2YjYsdmC2KkpCi0g2YXZgtmE2YXYqSDYqtit2KrZiNmKINi52YTZiSDZhdiz2LfYsdipICsgYnJhaWFlICsgMiDYrNmI2YXYqSArIDMg2KPZgtmE2KfZhSDYsdi12KfYtSArIDMg2KPZgtmE2KfZhSDYrNin2YEK2LbZhtio2YLZiiDYrNiy2KEg2YXZhiDZhdiz2KrZgtio2YTZh9mF"

$campImagePath = Join-Path $imgFolder "campaign_school.jpg"
if (-not (Test-Path $campImagePath)){ Fail "Missing campaign_school.jpg" }

$campClient = [System.Net.Http.HttpClient]::new()
$campClient.BaseAddress = [Uri]$base
$campForm = [System.Net.Http.MultipartFormDataContent]::new()

function Add-CampField($n,$v){
  $sc = [System.Net.Http.StringContent]::new($v,[System.Text.UTF8Encoding]::UTF8,"text/plain")
  $sc.Headers.Remove("Content-Type")
  $sc.Headers.Add("Content-Disposition","form-data; name=`"$n`"")
  $campForm.Add($sc)
}

Add-CampField "Title" $campTitle
Add-CampField "Description" $campDesc
Add-CampField "Duration" "30"
Add-CampField "GoalAmount" "100000"
Add-CampField "IsInKindDonation" "false"
Add-CampField "Category" "Education"
Add-CampField "CharityID" "$charityId"

$campStream = [System.IO.File]::OpenRead($campImagePath)
$campImgContent = [System.Net.Http.StreamContent]::new($campStream)
$campImgContent.Headers.ContentType = [System.Net.Http.Headers.MediaTypeHeaderValue]::Parse("image/jpeg")
$campImgContent.Headers.Add("Content-Disposition",'form-data; name="Image"; filename="campaign.jpg"')
$campForm.Add($campImgContent)

$campReq = [System.Net.Http.HttpRequestMessage]::new([System.Net.Http.HttpMethod]::Post,"api/Campaign/CreateCampaign")
$campReq.Headers.Authorization = [System.Net.Http.Headers.AuthenticationHeaderValue]::new("Bearer",$charLogin.token)
$campReq.Content = $campForm
$campResp = $campClient.SendAsync($campReq).Result
$campStream.Dispose()
if (-not $campResp.IsSuccessStatusCode){ Fail "Campaign creation failed" }
Write-Host "Campaign created." -ForegroundColor Green

Write-Section "Verify campaign"
try {
  $camps = Invoke-RestMethod -Uri "$base/api/charities/$charityId/campaigns" -Method Get -ErrorAction Stop
  if ($camps -and $camps.Count -gt 0){ Write-Host "Verification OK: Campaign present (Id=$($camps[0].Id))." -ForegroundColor Green }
  else { Write-Host "WARNING: No campaigns found." -ForegroundColor Yellow }
} catch { Write-Host "Campaign verification skipped" -ForegroundColor Yellow }

Write-Section "Flow complete"
Write-Host "CharityId=$charityId operations succeeded." -ForegroundColor Cyan
