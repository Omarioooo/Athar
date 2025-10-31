using AtharPlatform.Dtos;
using AtharPlatform.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AtharPlatform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharitiesController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly Context _db;

        public CharitiesController(IUnitOfWork uow, Context db)
        {
            _uow = uow;
            _db = db;
        }

        private int? GetCurrentUserId()
        {
            var idStr = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(idStr, out var id)) return id;
            return null;
        }

        // (GET) /api/charities?query=&page=1&pageSize=12
        [HttpGet]
    public async Task<ActionResult<PaginatedResultDto<CharityCardDto>>> GetAll([FromQuery] string? query, [FromQuery] int page = 1, [FromQuery] int pageSize = 12)
        {


            if (page <= 0 || pageSize <= 0)
                return BadRequest("Page and PageSize must be greater than zero.");



            var total = await _uow.Charity.CountAsync(query);

            if (total == 0)// Handle empty data
                return Ok(new PaginatedResultDto<CharityCardDto>
                {
                    Items = new List<CharityCardDto>(),
                    Page = page,
                    PageSize = pageSize,
                    Total = 0
                });


            var items = await _uow.Charity.GetPageAsync(query, page, pageSize);

            var dto = new PaginatedResultDto<CharityCardDto>
            {
                Items = items.Select(c => new CharityCardDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Image = c.Image,
                    ImageUrl = c.ScrapedInfo != null ? c.ScrapedInfo.ImageUrl : null,
                    ExternalWebsiteUrl = c.ScrapedInfo != null ? c.ScrapedInfo.ExternalWebsiteUrl : null,
                    CampaignsCount = c.campaigns?.Count ?? 0
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
            var c = await _uow.Charity.GetWithCampaignsAsync(id);
            if (c == null) return NotFound("Charity not found.");

            var dto = new CharityCardDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Image = c.Image,
                ImageUrl = c.ScrapedInfo != null ? c.ScrapedInfo.ImageUrl : null,
                ExternalWebsiteUrl = c.ScrapedInfo != null ? c.ScrapedInfo.ExternalWebsiteUrl : null,
                Campaigns = (c.campaigns ?? new()).Select(x => new MiniCampaignDto
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
                Image = c.Image,
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
            var items = await _uow.Charity.GetPageAsync(query, 1, take);
            return Ok(items.Select(c => new CharityCardDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Image = c.Image,
                ImageUrl = c.ScrapedInfo != null ? c.ScrapedInfo.ImageUrl : null,
                ExternalWebsiteUrl = c.ScrapedInfo != null ? c.ScrapedInfo.ExternalWebsiteUrl : null,
                CampaignsCount = c.campaigns?.Count ?? 0
            }));
        }

        // (POST) /api/charities  - create manually by Charity Admin or Super Admin
        [HttpPost]
        [Authorize(Roles = "CharityAdmin,SuperAdmin")]
        public async Task<IActionResult> Create([FromBody] CharityCreateDto body)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var entity = new Models.Charity
            {
                Name = body.Name,
                Description = body.Description,
                Image = body.Image,
                IsScraped = false
            };

            await _uow.Charity.AddAsync(entity);
            await _uow.SaveAsync();
            return CreatedAtAction(nameof(GetById), new { id = entity.Id }, new { entity.Id });
        }

        // (POST) /api/charities/import - bulk import scraped data
        [HttpPost("import")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Import([FromBody] IEnumerable<CharityImportItemDto> items)
        {
            if (items == null) return BadRequest("No data provided");
            var now = DateTime.UtcNow;
            var entities = new List<Models.Charity>();
            foreach (var i in items.Where(i => !string.IsNullOrWhiteSpace(i.Name)))
            {
                var charity = new Models.Charity
                {
                    Name = i.Name.Trim(),
                    Description = i.Description ?? string.Empty,
                    Image = i.Image, // if provided as bytes/base64 decoded by client
                    IsScraped = true,
                    ExternalId = i.ExternalId,
                    ImportedAt = now
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

            await _uow.Charity.BulkImportAsync(entities);
            await _uow.SaveAsync();
            return Ok(new { imported = entities.Count });
        }

        // (PUT) /api/charities/{id} - update charity (name/description/image and optional external links)
        [HttpPut("{id:int}")]
        [Authorize(Roles = "CharityAdmin,SuperAdmin")]
        public async Task<IActionResult> Update(int id, [FromBody] CharityUpdateDto body)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var charity = await _db.Charities.Include(c => c.ScrapedInfo).FirstOrDefaultAsync(c => c.Id == id);
            if (charity == null) return NotFound("Charity not found.");
            if (!charity.IsActive) return BadRequest("Charity is deactivated.");

            // Restrict CharityAdmin to only update their own charity
            if (User.IsInRole("CharityAdmin"))
            {
                var uid = GetCurrentUserId();
                if (uid == null || uid.Value != id)
                    return Forbid();
            }

            charity.Name = body.Name ?? charity.Name;
            if (body.Description != null) charity.Description = body.Description;
            if (body.Image != null && body.Image.Length > 0) charity.Image = body.Image;

            if (body.ImageUrl != null || body.ExternalWebsiteUrl != null)
            {
                if (charity.ScrapedInfo == null)
                    charity.ScrapedInfo = new CharityExternalInfo { CharityId = charity.Id };
                if (body.ImageUrl != null) charity.ScrapedInfo.ImageUrl = body.ImageUrl;
                if (body.ExternalWebsiteUrl != null) charity.ScrapedInfo.ExternalWebsiteUrl = body.ExternalWebsiteUrl;
            }

            await _uow.SaveAsync();
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
            await _uow.SaveAsync();
            return NoContent();
        }

        // Approve / Reject (SuperAdmin only)
        [HttpPost("{id:int}/approve")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Approve(int id)
        {
            var charity = await _db.Charities.FirstOrDefaultAsync(c => c.Id == id);
            if (charity == null) return NotFound("Charity not found.");
            charity.Status = Models.Enums.CharityStatusEnum.Approved;
            charity.IsActive = true;
            charity.DeactivatedAt = null;
            await _uow.SaveAsync();
            return Ok(new { id, status = charity.Status.ToString() });
        }

        [HttpPost("{id:int}/reject")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Reject(int id)
        {
            var charity = await _db.Charities.FirstOrDefaultAsync(c => c.Id == id);
            if (charity == null) return NotFound("Charity not found.");
            charity.Status = Models.Enums.CharityStatusEnum.Rejected;
            await _uow.SaveAsync();
            return Ok(new { id, status = charity.Status.ToString() });
        }

        // (GET) /api/charities/stats?charityId=123 - global if omitted, per charity if provided
        [HttpGet("stats")]
        [Authorize(Roles = "CharityAdmin,SuperAdmin")]
        public async Task<ActionResult<CharityStatsDto>> GetStats([FromQuery] int? charityId = null)
        {
            var now = DateTime.UtcNow;
            var last30 = now.AddDays(-30);

            // If CharityAdmin, force stats to own charity
            if (User.IsInRole("CharityAdmin"))
            {
                var uid = GetCurrentUserId();
                if (uid == null) return Forbid();
                charityId = uid.Value;
            }

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
                ImageUrl = c.ImageUrl,
                GoalAmount = c.GoalAmount,
                RaisedAmount = c.RaisedAmount,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                Category = c.Category,
                CharityName = c.Charity?.Name
            });
            return Ok(result);
        }

        // (GET) /api/charities/{id}/material-donations
        [HttpGet("{id:int}/material-donations")]
        [Authorize(Roles = "CharityAdmin,SuperAdmin")]
        public async Task<ActionResult<IEnumerable<MaterialDonationDto>>> GetMaterialDonationsByCharity(int id)
        {
            // CharityAdmin can only view own material donations
            if (User.IsInRole("CharityAdmin"))
            {
                var uid = GetCurrentUserId();
                if (uid == null || uid.Value != id)
                    return Forbid();
            }
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
        [HttpGet("{id:int}/image")]
        [AllowAnonymous]
        public async Task<IActionResult> GetImage(int id)
        {
            var charity = await _uow.Charity.GetAsync(id);
            if (charity == null || charity.Image == null || charity.Image.Length == 0)
                return NotFound();

            // If you later store content type, use it here. Default to JPEG.
            return File(charity.Image, "image/jpeg");
        }
    }
}
