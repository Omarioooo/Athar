using System.Security.Claims;
using System.Text;
using AtharPlatform.DTO;
using AtharPlatform.Dtos;
using AtharPlatform.DTOs;
using AtharPlatform.Models.Enum;
using AtharPlatform.Repositories;
using AtharPlatform.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AtharPlatform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharitiesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountContextService _accountContextService;
        private readonly Context _db;
        private readonly UserManager<UserAccount> _userManager;
        private readonly INotificationService _notificationService;

        public CharitiesController(IUnitOfWork unitOfWork, IAccountContextService accountContextService
            , Context db, UserManager<UserAccount> userManager, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _accountContextService = accountContextService;
            _db = db;
            _userManager = userManager;
            _notificationService = notificationService;
        }

        // Helper method to convert relative URLs to full URLs
        private string? ToFullUrl(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return null;
            
            // If already a full URL (external), return as-is
            if (imageUrl.StartsWith("http://") || imageUrl.StartsWith("https://"))
                return imageUrl;
            
            // Convert relative path to full URL
            var request = HttpContext.Request;
            // Force HTTPS scheme
            var baseUrl = $"https://{request.Host}";
            return $"{baseUrl}{imageUrl}";
        }




        // فتح باب التطوع
        [HttpPost("open-volunteers/{charityId}")]
        //[Authorize(Roles = "CharityAdmin")]
        public async Task<IActionResult> OpenVolunteers(int charityId)
        {
            try
            {
                var record = (await _unitOfWork.CharityVolunteers.GetByCharityIdAsync(charityId))
                             .FirstOrDefault();

                if (record != null && record.IsOpen)
                    return BadRequest(new { Message = "Volunteers are already open for this charity." });

                if (record == null)
                {
                    // إنشاء record جديد
                    record = new CharityVolunteer
                    {
                        CharityId = charityId,
                        Date = DateTime.UtcNow,
                        IsOpen = true
                    };
                    await _unitOfWork.CharityVolunteers.AddAsync(record);
                }
                else
                {
                    // تحديث record موجود
                    record.IsOpen = true;
                    record.Date = DateTime.UtcNow;
                }

                await _unitOfWork.SaveAsync();

                return Ok(new
                {
                    Message = "Volunteers are now open.",
                    CharityId = charityId,
                    OpenedAt = record.Date
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Failed to open volunteers.", Error = ex.Message });
            }
        }

        // غلق باب التطوع
        [HttpPost("close-volunteers/{charityId}")]
       //[Authorize(Roles = "CharityAdmin")]
        public async Task<IActionResult> CloseVolunteers(int charityId)
        {
            try
            {
                var record = (await _unitOfWork.CharityVolunteers.GetByCharityIdAsync(charityId))
                             .FirstOrDefault();

                if (record == null || !record.IsOpen)
                    return BadRequest(new { Message = "Volunteers are already closed for this charity." });

                record.IsOpen = false;
                await _unitOfWork.SaveAsync();

                return Ok(new
                {
                    Message = "Volunteers are now closed.",
                    CharityId = charityId,
                    ClosedAt = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Failed to close volunteers.", Error = ex.Message });
            }
        }





        // فتح باب العروض للـ Vendor
        [HttpPost("open-vendor-offers/{charityId}")]
       // [Authorize(Roles = "CharityAdmin")]
        public async Task<IActionResult> OpenVendorOffers(int charityId)
        {
            try
            {
                // جلب record موجود للجمعية
                var record = (await _unitOfWork.CharityVendorOffers.GetByCharityIdAsync(charityId))
                             .FirstOrDefault();

                if (record != null && record.IsOpen)
                    return BadRequest(new { Message = "Vendor offers are already open for this charity." });

                if (record == null)
                {
                    // إنشاء record جديد
                    record = new CharityVendorOffer
                    {
                        CharityId = charityId,
                        Date = DateTime.UtcNow,
                        IsOpen = true
                    };
                    await _unitOfWork.CharityVendorOffers.AddAsync(record);
                }
                else
                {
                    // تحديث record موجود
                    record.IsOpen = true;
                    record.Date = DateTime.UtcNow;
                }

                await _unitOfWork.SaveAsync();

                return Ok(new
                {
                    Message = "Vendor offers are now open.",
                    CharityId = charityId,
                    OpenedAt = record.Date
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Failed to open vendor offers.", Error = ex.Message });
            }
        }

        // غلق باب العروض للـ Vendor
        [HttpPost("close-vendor-offers/{charityId}")]
       // [Authorize(Roles = "CharityAdmin")]
        public async Task<IActionResult> CloseVendorOffers(int charityId)
        {
            try
            {
                // جلب record موجود للجمعية
                var record = (await _unitOfWork.VendorOffers.GetByCharityIdAsync(charityId))
                             .FirstOrDefault();

                if (record == null || !record.IsOpen)
                    return BadRequest(new { Message = "Vendor offers are already closed for this charity." });

                record.IsOpen = false;
                await _unitOfWork.SaveAsync();

                return Ok(new
                {
                    Message = "Vendor offers are now closed.",
                    CharityId = charityId,
                    ClosedAt = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Failed to close vendor offers.", Error = ex.Message });
            }
        }






        // (GET) /api/charities?query=&page=1&pageSize=12
        [HttpGet]
        public async Task<ActionResult<AtharPlatform.Dtos.PaginatedResultDto<CharityCardDto>>> GetAll(
            [FromQuery] string? query, 
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 12, 
            [FromQuery] bool includeCampaigns = false,
            [FromQuery] bool? isActive = null,
            [FromQuery] bool? isScraped = null,
            [FromQuery] bool? hasExternalWebsite = null)
        {
            // Public endpoint; no authentication required

            if (page <= 0 || pageSize <= 0)
                return BadRequest("Page and PageSize must be greater than zero.");



            var total = await _unitOfWork.Charities.CountAsync(
                query, 
                isActive, 
                isScraped, 
                hasExternalWebsite);

            if (total == 0)// Handle empty data
                return Ok(new AtharPlatform.Dtos.PaginatedResultDto<CharityCardDto>
                {
                    Items = new List<CharityCardDto>(),
                    Page = page,
                    PageSize = pageSize,
                    Total = 0
                });


            var items = await _unitOfWork.Charities.GetPageAsync(
                query, 
                page, 
                pageSize,
                isActive,
                isScraped,
                hasExternalWebsite);

            // When includeCampaigns=true, enrich each charity with campaigns derived from:
            // 1) Direct FK (Campaign.CharityID == Charity.Id)
            // 2) Indirect support: Campaign.SupportingCharitiesJson contains the charity name
            List<(int Id, string Name,/* byte[]? Image,*/ string? ImageUrl, string? ExternalWebsiteUrl, string Description)> pageCharities = items
                // Use the charity primary key directly; Account may not be eagerly loaded in GetPageAsync
                .Select(c => (c.Id, c.Name, /*c.Image,*/ 
                               ToFullUrl(c.ImageUrl ?? (c.ScrapedInfo != null ? c.ScrapedInfo.ImageUrl : null)),
                               c.ScrapedInfo != null ? c.ScrapedInfo.ExternalWebsiteUrl : null, c.Description))
                .ToList();

            var campaignMap = new Dictionary<int, List<MiniCampaignDto>>();
            if (includeCampaigns)
            {
                // Load minimal campaign fields once
                var allCamps = await _db.Campaigns
                    .AsNoTracking()
                    .Select(x => new
                    {
                        x.Id,
                        x.Title,
                        x.GoalAmount,
                        x.RaisedAmount,
                        x.CharityID,
                        x.SupportingCharitiesJson
                    })
                    .ToListAsync();

                static IEnumerable<string> ParseSupporters(string? json)
                {
                    if (string.IsNullOrWhiteSpace(json)) return Array.Empty<string>();
                    try
                    {
                        var list = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<string>>(json);
                        return list?.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()) ?? Array.Empty<string>();
                    }
                    catch { return Array.Empty<string>(); }
                }

                bool NameMatches(string supporter, string charityName)
                {
                    if (string.IsNullOrWhiteSpace(supporter) || string.IsNullOrWhiteSpace(charityName)) return false;
                    var s = supporter.Trim();
                    var n = charityName.Trim();
                    // exact or contains in either direction (basic fuzzy)
                    return string.Equals(s, n, StringComparison.Ordinal) || n.Contains(s) || s.Contains(n);
                }

                foreach (var (Id, Name, _, _, _) in pageCharities)
                {
                    var set = new Dictionary<int, MiniCampaignDto>();

                    // FK-linked campaigns
                    foreach (var c in allCamps.Where(c => c.CharityID == Id))
                    {
                        if (!set.ContainsKey(c.Id))
                            set[c.Id] = new MiniCampaignDto { Id = c.Id, Title = c.Title, GoalAmount = c.GoalAmount, RaisedAmount = c.RaisedAmount };
                    }

                    // Support-derived campaigns
                    foreach (var c in allCamps)
                    {
                        var supporters = ParseSupporters(c.SupportingCharitiesJson);
                        if (supporters.Any(s => NameMatches(s, Name)))
                        {
                            if (!set.ContainsKey(c.Id))
                                set[c.Id] = new MiniCampaignDto { Id = c.Id, Title = c.Title, GoalAmount = c.GoalAmount, RaisedAmount = c.RaisedAmount };
                        }
                    }

                    campaignMap[Id] = set.Values.ToList();
                }
            }

            var dto = new AtharPlatform.Dtos.PaginatedResultDto<CharityCardDto>
            {
                Items = pageCharities.Select(pc => new CharityCardDto
                {
                    Id = pc.Id,
                    Name = pc.Name,
                    Description = pc.Description,
                   // Image = pc.Image,
                    ImageUrl = pc.ImageUrl,
                    ExternalWebsiteUrl = pc.ExternalWebsiteUrl,
                    CampaignsCount = includeCampaigns ? (campaignMap.TryGetValue(pc.Id, out var list) ? list.Count : 0) : 0,
                    Campaigns = includeCampaigns && campaignMap.TryGetValue(pc.Id, out var lst)
                        ? lst
                        : Array.Empty<MiniCampaignDto>()
                }),
                Page = page,
                PageSize = pageSize,
                Total = total
            };
            return Ok(dto);
        }

        // (GET) /api/charities/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CharityCardDto>> GetById(int id)
        {
            // Public endpoint; no authentication required

            var c = await _unitOfWork.Charities.GetWithCampaignsAsync(id);
            if (c == null) return NotFound("Charity not found.");

            var dto = new CharityCardDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
               // Image = c.Image,
                ImageUrl = c.ScrapedInfo != null ? c.ScrapedInfo.ImageUrl : null,
                ExternalWebsiteUrl = c.ScrapedInfo != null ? c.ScrapedInfo.ExternalWebsiteUrl : null,
                Campaigns = (c.Campaigns ?? new()).Select(x => new MiniCampaignDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    GoalAmount = x.GoalAmount,
                    RaisedAmount = x.RaisedAmount
                })
            };
            return Ok(dto);
        }

        // (GET) /api/charities/{name}
        [HttpGet("byName/{name}")]
        public async Task<ActionResult<CharityCardDto>> GetByName(string name)
        {
            // Public endpoint; no authentication required

            if (string.IsNullOrWhiteSpace(name)) return BadRequest("Name is required.");
            var trimmed = name.Trim();
            // Try exact match first
            var c = await _db.Charities
                .Include(x => x.ScrapedInfo)
                .FirstOrDefaultAsync(x => x.Name == trimmed);

            // Fallback to partial contains match (useful for inputs like "مرسال")
            if (c == null)
            {
                c = await _db.Charities
                    .Include(x => x.ScrapedInfo)
                    .Where(x => x.Name.Contains(trimmed))
                    .OrderBy(x => x.Name.Length)
                    .FirstOrDefaultAsync();
            }

            if (c == null) return NotFound("Charity not found.");

            var dto = new CharityCardDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
               // Image = c.Image,
                ImageUrl = c.ScrapedInfo != null ? c.ScrapedInfo.ImageUrl : null,
                ExternalWebsiteUrl = c.ScrapedInfo != null ? c.ScrapedInfo.ExternalWebsiteUrl : null
            };
            return Ok(dto);
        }

        // (GET) /api/charities/search?query=
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<CharityCardDto>>> Search([FromQuery] string query, [FromQuery] int take = 10)
        {
            if (string.IsNullOrWhiteSpace(query)) return Ok(Array.Empty<CharityCardDto>());
            var items = await _unitOfWork.Charities.GetPageAsync(query, 1, take);
            return Ok(items.Select(c => new CharityCardDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
               // Image = c.Image,
                ImageUrl = c.ScrapedInfo != null ? c.ScrapedInfo.ImageUrl : null,
                ExternalWebsiteUrl = c.ScrapedInfo != null ? c.ScrapedInfo.ExternalWebsiteUrl : null,
                CampaignsCount = c.Campaigns?.Count ?? 0
            }));
        }

        // (POST) /api/charities/import - bulk import scraped data
        [HttpPost("import")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Import([FromBody] IEnumerable<CharityImportItemDto> items)
        {
            if (items == null) return BadRequest("No data provided");
            var now = DateTime.UtcNow;
            var entities = new List<Models.Charity>();
            foreach (var i in items.Where(i => !string.IsNullOrWhiteSpace(i.Name)))
            {
                // Create placeholder Identity account to satisfy shared PK FK (Charity.Id == UserAccount.Id)
                var uname = (i.Name ?? $"charity_{Guid.NewGuid():N}").Trim().Replace(" ", "_");
                var email = $"{uname.ToLowerInvariant()}@charity.local";
                var account = new UserAccount
                {
                    UserName = uname,
                    Email = email,
                    EmailConfirmed = true
                };

                // Use a strong random password to satisfy Identity requirements
                var pwd = $"{Guid.NewGuid():N}!Aa1";
                var createRes = await _userManager.CreateAsync(account, pwd);
                if (!createRes.Succeeded)
                {
                    // If username/email collision, append random suffix and retry once
                    uname = $"{uname}_{Guid.NewGuid().ToString("N")[..6]}";
                    email = $"{uname.ToLowerInvariant()}@charity.local";
                    account.UserName = uname;
                    account.Email = email;
                    createRes = await _userManager.CreateAsync(account, pwd);
                    if (!createRes.Succeeded)
                        return BadRequest(new { message = "Failed to create identity account for charity.", errors = createRes.Errors });
                }

                var charity = new Models.Charity
                {
                    Name = (i.Name ?? string.Empty).Trim(),
                    Description = i.Description ?? string.Empty,
                    IsScraped = true,
                    ExternalId = i.ExternalId,
                    ImportedAt = now,
                    Id = account.Id,
                    Account = account,
                    // Provide a placeholder verification document for scraped data to satisfy [Required]
                    VerificationDocument = Array.Empty<byte>()
                };

                if (i.ImageUrl != null || i.ExternalWebsiteUrl != null)
                {
                    charity.ScrapedInfo = new Models.CharityExternalInfo
                    {
                        ImageUrl = i.ImageUrl,
                        ExternalWebsiteUrl = i.ExternalWebsiteUrl
                    };
                }

                entities.Add(charity);
            }

            await _unitOfWork.Charities.BulkImportAsync(entities);
            await _unitOfWork.SaveAsync();
            return Ok(new { imported = entities.Count });
        }

        // (PUT) /api/charities/{id} - update charity (name/description/image and optional external links)
        [HttpPut("{id:int}")]
       // [Authorize(Roles = "CharityAdmin,SuperAdmin")]
        public async Task<IActionResult> Update(int id, [FromBody] CharityUpdateDto body)
        {
            var userId = _accountContextService.GetCurrentAccountId();

            if (!ModelState.IsValid) return BadRequest(ModelState);
            var charity = await _db.Charities.Include(c => c.ScrapedInfo).FirstOrDefaultAsync(c => c.Id == id);
            if (charity == null) return NotFound("Charity not found.");
            if (!charity.IsActive) return BadRequest("Charity is deactivated.");

            charity.Name = body.Name ?? charity.Name;
            if (body.Description != null) charity.Description = body.Description;


            if (body.ImageUrl != null || body.ExternalWebsiteUrl != null)
            {
                if (charity.ScrapedInfo == null)
                    charity.ScrapedInfo = new CharityExternalInfo { CharityId = charity.Id };
                if (body.ImageUrl != null) charity.ScrapedInfo.ImageUrl = body.ImageUrl;
                if (body.ExternalWebsiteUrl != null) charity.ScrapedInfo.ExternalWebsiteUrl = body.ExternalWebsiteUrl;
            }

            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        // (DELETE) /api/charities/{id} - soft delete / deactivate
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var charity = await _db.Charities.FirstOrDefaultAsync(c => c.Id == id);
            if (charity == null) return NotFound("Charity not found.");
            if (!charity.IsActive) return NoContent();

            charity.IsActive = false;
            charity.DeactivatedAt = DateTime.UtcNow;
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        // Approve / Reject (SuperAdmin only)
        [HttpPost("{id:int}/approve")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Approve(int id)
        {
            var charity = await _db.Charities.FirstOrDefaultAsync(c => c.Id == id);
            if (charity == null) return NotFound("Charity not found.");
            charity.Status = Models.Enums.CharityStatusEnum.Approved;
            charity.IsActive = true;
            charity.DeactivatedAt = null;
            await _unitOfWork.SaveAsync();


            // Get admin as sender
            var admins = await _userManager.GetUsersInRoleAsync("SuperAdmin");
            var adminId = admins.FirstOrDefault()?.Id;

            

            // Receivers = charity owner
            var receivers = new List<int> { charity.Id };

            await _notificationService.SendNotificationAsync(
                adminId.Value,
                receivers,
                NotificationsTypeEnum.AdminApproved
            );

            return Ok(new { id, status = charity.Status.ToString() });
        }

        [HttpPost("{id:int}/reject")]
       // [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Reject(int id)
        {
            var charity = await _db.Charities.FirstOrDefaultAsync(c => c.Id == id);
            if (charity == null) return NotFound("Charity not found.");
            charity.Status = Models.Enums.CharityStatusEnum.Rejected;
            await _unitOfWork.SaveAsync();


            
            var admins = await _userManager.GetUsersInRoleAsync("SuperAdmin");
            var admin = admins.FirstOrDefault();

            

            var adminId = admin.Id;

            
            var receivers = new List<int> { charity.Id };

            await _notificationService.SendNotificationAsync(
                adminId,
                receivers,
                NotificationsTypeEnum.AdminRejected
            );

            return Ok(new { id, status = charity.Status.ToString() });
        }

        // (GET) /api/charities/stats?charityId=123 - global if omitted, per charity if provided
        [HttpGet("stats")]
        //[Authorize(Roles = "CharityAdmin,SuperAdmin")]
        public async Task<ActionResult<CharityStatsDto>> GetStats([FromQuery] int? charityId = null)
        {
            var userId = _accountContextService.GetCurrentAccountId();

            var now = DateTime.UtcNow;
            var last30 = now.AddDays(-30);



            IQueryable<Charity> charities = _db.Charities;
            IQueryable<Campaign> campaigns = _db.Campaigns.Include(c => c.Charity);
            IQueryable<CampaignDonation> campaignDonations = _db.CampaignDonations.Include(cd => cd.Donation).Include(cd => cd.Campaign);
            IQueryable<CharityDonation> charityDonations = _db.CharityDonations.Include(cd => cd.Donation).Include(cd => cd.Charity);
            IQueryable<MaterialDonation> materialDonations = _db.MaterialDonations.Include(x => x.CharityMaterialDonation);

            if (charityId.HasValue)
            {
                charities = charities.Where(c => c.Id == charityId.Value);
                campaigns = campaigns.Where(c => c.CharityID == charityId.Value);
                campaignDonations = campaignDonations.Where(cd => cd.Campaign.CharityID == charityId.Value);
                charityDonations = charityDonations.Where(cd => cd.charityID == charityId.Value);
                materialDonations = materialDonations.Where(md => md.CharityMaterialDonation.CharityId == charityId.Value);
            }

            var totalCharities = await charities.CountAsync();
            var approved = await charities.CountAsync(c => c.Status == Models.Enums.CharityStatusEnum.Approved);
            var pending = await charities.CountAsync(c => c.Status == Models.Enums.CharityStatusEnum.Pending);
            var rejected = await charities.CountAsync(c => c.Status == Models.Enums.CharityStatusEnum.Rejected);

            var totalCampaigns = await campaigns.CountAsync();
            var activeCampaigns = await campaigns.CountAsync(c => c.Status == Models.Enum.CampainStatusEnum.inProgress);

            var totalCash = (await campaignDonations.SumAsync(cd => (decimal?)cd.Donation.NetAmountToCharity) ?? 0m)
                          + (await charityDonations.SumAsync(cd => (decimal?)cd.Donation.NetAmountToCharity) ?? 0m);

            var last30Cash = (await campaignDonations.Where(cd => cd.Donation.CreatedAt >= last30)
                                .SumAsync(cd => (decimal?)cd.Donation.NetAmountToCharity) ?? 0m)
                           + (await charityDonations.Where(cd => cd.Donation.CreatedAt >= last30)
                                .SumAsync(cd => (decimal?)cd.Donation.NetAmountToCharity) ?? 0m);

            var materialCount = await materialDonations.CountAsync();

            // Average donation amount and count (campaign + charity donations)
            var allDonations = campaignDonations.Select(cd => cd.Donation)
                                .Concat(charityDonations.Select(cd => cd.Donation));
            var donationsCount = await allDonations.CountAsync();
            decimal avgDonation = 0m;
            if (donationsCount > 0)
            {
                var total = await allDonations.SumAsync(d => (decimal?)d.NetAmountToCharity) ?? 0m;
                avgDonation = total / donationsCount;
            }

            // Top campaigns by total donations (sum of NetAmountToCharity)
            var topCampaigns = await campaignDonations
                .GroupBy(cd => new { cd.CampaignId, cd.Campaign.Title })
                .Select(g => new TopCampaignDto
                {
                    Id = g.Key.CampaignId,
                    Title = g.Key.Title,
                    TotalDonations = g.Sum(x => (decimal?)x.Donation.NetAmountToCharity) ?? 0m,
                    DonationsCount = g.Count()
                })
                .OrderByDescending(x => x.TotalDonations)
                .Take(5)
                .ToListAsync();

            var dto = new CharityStatsDto
            {
                TotalCharities = totalCharities,
                ApprovedCharities = approved,
                PendingCharities = pending,
                RejectedCharities = rejected,
                TotalCampaigns = totalCampaigns,
                ActiveCampaigns = activeCampaigns,
                TotalCashDonations = totalCash,
                Last30DaysCash = last30Cash,
                TotalMaterialDonations = materialCount,
                AverageDonationAmount = avgDonation,
                DonationsCount = donationsCount,
                TopCampaigns = topCampaigns
            };

            return Ok(dto);
        }

        // (GET) /api/charities/{id}/campaigns
        [HttpGet("{id:int}/campaigns")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CampaignDto>>> GetCampaignsByCharity(int id)
        {
            var items = await _db.Campaigns.Include(c => c.Charity).Where(c => c.CharityID == id).ToListAsync();
            var result = items.Select(c => new CampaignDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                ImageUrl = ToFullUrl(c.ImageUrl),
                GoalAmount = c.GoalAmount,
                RaisedAmount = c.RaisedAmount,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                Category = c.Category,
                CharityName = c.Charity?.Name
            });
            return Ok(result);
        }

        // (GET) /api/charities/campaign-counts
        // Lightweight payload for dashboards: [{ charity_id, charity_name, campaigns_count }]
        [HttpGet("campaign-counts")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CharityCountDto>>> GetCampaignCounts()
        {
            // Compute counts including FK-linked AND supporter-derived campaigns
            var charities = await _db.Charities
                .AsNoTracking()
                .Where(c => c.IsActive)
                .Select(c => new { c.Id, c.Name })
                .ToListAsync();

            var allCamps = await _db.Campaigns
                .AsNoTracking()
                .Select(x => new { x.Id, x.CharityID, x.SupportingCharitiesJson })
                .ToListAsync();

            static IEnumerable<string> ParseSupporters(string? json)
            {
                if (string.IsNullOrWhiteSpace(json)) return Array.Empty<string>();
                try
                {
                    var list = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<string>>(json);
                    return list?.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()) ?? Array.Empty<string>();
                }
                catch { return Array.Empty<string>(); }
            }

            static bool NameMatches(string supporter, string charityName)
            {
                if (string.IsNullOrWhiteSpace(supporter) || string.IsNullOrWhiteSpace(charityName)) return false;
                var s = supporter.Trim();
                var n = charityName.Trim();
                return string.Equals(s, n, StringComparison.Ordinal) || n.Contains(s) || s.Contains(n);
            }

            var result = new List<CharityCountDto>(charities.Count);
            foreach (var ch in charities)
            {
                var set = new HashSet<int>();

                // FK-linked
                foreach (var c in allCamps.Where(c => c.CharityID == ch.Id)) set.Add(c.Id);

                // Support-derived
                foreach (var c in allCamps)
                {
                    var supporters = ParseSupporters(c.SupportingCharitiesJson);
                    if (supporters.Any(s => NameMatches(s, ch.Name))) set.Add(c.Id);
                }

                result.Add(new CharityCountDto
                {
                    CharityId = ch.Id,
                    CharityName = ch.Name,
                    CampaignsCount = set.Count
                });
            }

            var ordered = result
                .OrderByDescending(x => x.CampaignsCount)
                .ThenBy(x => x.CharityName)
                .ToList();

            return Ok(ordered);
        }

        // (GET) /api/charities/campaigns-summary?includeCampaigns=true&format=json
        // Returns per-charity counts and optionally their campaign list. When format=csv, returns a CSV file.
        // IMPORTANT: This summary includes campaigns linked by FK AND campaigns where the charity appears in Campaign.SupportingCharitiesJson.
        [HttpGet("campaigns-summary")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCampaignsSummary([FromQuery] bool includeCampaigns = true, [FromQuery] string format = "json")
        {
            // Load active charities (id + name) ordered by name
            var charities = await _db.Charities
                .AsNoTracking()
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .Select(c => new { c.Id, c.Name })
                .ToListAsync();

            // Load minimal campaign fields once
            var allCamps = await _db.Campaigns
                .AsNoTracking()
                .Select(x => new
                {
                    x.Id,
                    x.Title,
                    x.GoalAmount,
                    x.RaisedAmount,
                    x.CharityID,
                    x.SupportingCharitiesJson
                })
                .ToListAsync();

            static IEnumerable<string> ParseSupporters(string? json)
            {
                if (string.IsNullOrWhiteSpace(json)) return Array.Empty<string>();
                try
                {
                    var list = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<string>>(json);
                    return list?.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()) ?? Array.Empty<string>();
                }
                catch { return Array.Empty<string>(); }
            }

            static bool NameMatches(string supporter, string charityName)
            {
                if (string.IsNullOrWhiteSpace(supporter) || string.IsNullOrWhiteSpace(charityName)) return false;
                var s = supporter.Trim();
                var n = charityName.Trim();
                // exact or contains in either direction (basic fuzzy)
                return string.Equals(s, n, StringComparison.Ordinal) || n.Contains(s) || s.Contains(n);
            }

            var dto = new List<CharityCampaignsSummaryDto>(charities.Count);

            foreach (var ch in charities)
            {
                var set = new Dictionary<int, MiniCampaignDto>();

                // FK-linked campaigns
                foreach (var c in allCamps.Where(c => c.CharityID == ch.Id))
                {
                    if (!set.ContainsKey(c.Id))
                        set[c.Id] = new MiniCampaignDto { Id = c.Id, Title = c.Title, GoalAmount = c.GoalAmount, RaisedAmount = c.RaisedAmount };
                }

                // Support-derived campaigns
                foreach (var c in allCamps)
                {
                    var supporters = ParseSupporters(c.SupportingCharitiesJson);
                    if (supporters.Any(s => NameMatches(s, ch.Name)))
                    {
                        if (!set.ContainsKey(c.Id))
                            set[c.Id] = new MiniCampaignDto { Id = c.Id, Title = c.Title, GoalAmount = c.GoalAmount, RaisedAmount = c.RaisedAmount };
                    }
                }

                dto.Add(new CharityCampaignsSummaryDto
                {
                    CharityId = ch.Id,
                    CharityName = ch.Name,
                    CampaignsCount = set.Count,
                    Campaigns = includeCampaigns ? set.Values.ToList() : Array.Empty<MiniCampaignDto>()
                });
            }

            if (!string.Equals(format, "csv", StringComparison.OrdinalIgnoreCase))
                return Ok(dto);

            // CSV export
            var sb = new StringBuilder();
            // Header
            sb.AppendLine("CharityName,CampaignsCount,CampaignTitles");
            foreach (var row in dto)
            {
                var titles = includeCampaigns ? string.Join(" | ", row.Campaigns.Select(x => x.Title)) : string.Empty;
                // Escape quotes and commas
                var charityName = EscapeCsv(row.CharityName);
                var titlesEscaped = EscapeCsv(titles);
                sb.AppendLine($"{charityName},{row.CampaignsCount},{titlesEscaped}");
            }

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "text/csv; charset=utf-8", fileDownloadName: $"charity_campaigns_summary_{DateTime.UtcNow:yyyyMMdd}.csv");
        }

        private static string EscapeCsv(string? value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            var needsQuotes = value.Contains(',') || value.Contains('"') || value.Contains('\n') || value.Contains('\r');
            var v = value.Replace("\"", "\"\"");
            return needsQuotes ? $"\"{v}\"" : v;
        }

        // (GET) /api/charities/{id}/material-donations
        [HttpGet("{id:int}/material-donations")]
       // [Authorize(Roles = "CharityAdmin,SuperAdmin")]
        public async Task<ActionResult<IEnumerable<MaterialDonationDto>>> GetMaterialDonationsByCharity(int id)
        {
            // CharityAdmin can only view own material donations

            var records = await _db.MaterialDonations
                .Include(m => m.CharityMaterialDonation)
                .Where(m => m.CharityMaterialDonation.CharityId == id)
                .ToListAsync();

            var result = records.Select(m => new MaterialDonationDto
            {
                Id = m.Id,
                DonorFirstName = m.DonorFirstName,
                DonorLastName = m.DonorLastName,
                PhoneNumber = m.PhoneNumber,
                ItemName = m.ItemName,
                Quantity = m.Quantity,
                MeasurementUnit = m.MeasurementUnit,
                Description = m.Description,
                Country = m.Country,
                City = m.City,
                Date = m.CharityMaterialDonation.Date
            });
            return Ok(result);
        }

        // (GET) /api/charities/{id}/image - serve manual image bytes as a browser-friendly URL
        //[HttpGet("{id:int}/image")]
        //[AllowAnonymous]
        //public async Task<IActionResult> GetImage(int id)
        //{
        //    var charity = await _unitOfWork.Charities.GetAsync(id);
        //    if (charity == null)
        //        return NotFound();

        //    // If you later store content type, use it here. Default to JPEG.
        //    if (charity.Image == null || charity.Image.Length == 0)
        //        return NotFound();
        //    return File(charity.Image, "image/jpeg");
        //}



    }
}
