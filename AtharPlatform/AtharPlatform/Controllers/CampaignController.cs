using AtharPlatform.Dtos;
using AtharPlatform.Models.Enum;
using AtharPlatform.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;


namespace AtharPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Context _context;

        public CampaignController(IUnitOfWork unitOfWork, Context context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }


        //public async Task<IActionResult> GetAllCampaign()
        //{
        //    var campaign = await _icampaignrepository.GetAllAsync();
        //    var result = campaign.Select(c => new CampaignDto
        //    {
        //        Id = c.Id,
        //        Title = c.Title,
        //        Description = c.Description,
        //        Image = c.Image,
        //        GoalAmount = c.GoalAmount,
        //        RaisedAmount = c.RaisedAmount,
        //        StartDate = c.StartDate,
        //        EndDate = c.EndDate,
        //        Category = c.Category,
        //        CharityName = c.Charity.Name


        //    });
        //    return Ok(result);

        //}
        [HttpGet("[action]")]
        public async Task<ActionResult<PaginatedResultDto<CampaignDto>>> GetAll([FromQuery] string? query, [FromQuery] int page = 1, [FromQuery] int pageSize = 12)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest("Page and PageSize must be greater than zero.");

            var q = _context.Campaigns.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                var term = query.Trim();
                q = q.Where(c =>
                    c.Title.Contains(term) ||
                    c.Description.Contains(term) ||
                    (c.Charity != null && c.Charity.Name.Contains(term)));
            }

            var total = await q.CountAsync();

            var items = await q
                .OrderBy(c => c.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CampaignDto
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
                    CharityName = c.Charity != null ? c.Charity.Name : null
                })
                .ToListAsync();

            return Ok(new PaginatedResultDto<CampaignDto>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                Total = total
            });
        }


        [HttpGet("[action]")]
        public async Task<IActionResult>GetCampaignById(int id)
        {
            var campaign = await _unitOfWork.Campaign.GetAsync(id);
            if (campaign == null)
                return NotFound("Campaign not found.");
            var result = new CampaignDto
            {
                Id = campaign.Id,
                Title = campaign.Title,
                Description = campaign.Description,
                ImageUrl = campaign.ImageUrl,
                GoalAmount = campaign.GoalAmount,
                RaisedAmount = campaign.RaisedAmount,
                StartDate = campaign.StartDate,
                EndDate = campaign.EndDate,
                Category = campaign.Category,
                CharityName = campaign.Charity?.Name
            };
            return Ok(result);

        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetByType(CampaignCategoryEnum type)
        {
            var campaign = await _unitOfWork.Campaign.GetByType(type);
            if (campaign == null || !campaign.Any())// it return list so I check if list is Empty
                return NotFound("Campaign not found.");
            var result = campaign.Select(c => new CampaignDto
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
                CharityName = c.Charity != null ? c.Charity.Name : null
            });
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetByDate([FromQuery] bool latestFirst = true)
        {
            var campaigns = await _unitOfWork.Campaign.GetByDateAsync(latestFirst);

            if (campaigns == null || !campaigns.Any())
                return NotFound("No campaigns found.");

            var result = campaigns.Select(c => new CampaignDto
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
                CharityName = c.Charity != null ? c.Charity.Name : null
            });

            return Ok(result);
        }

        // (GET) /api/Campaign/scraped?query=&page=1&pageSize=12
        // Returns campaigns in the original scraped JSON shape (snake_case) including supporting_charities and external_id
        [HttpGet("scraped")]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedResultDto<CampaignScrapedDto>>> GetScraped([FromQuery] string? query, [FromQuery] int page = 1, [FromQuery] int pageSize = 12)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest("Page and PageSize must be greater than zero.");

            var q = _context.Campaigns.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(query))
            {
                var term = query.Trim();
                q = q.Where(c =>
                    c.Title.Contains(term) ||
                    c.Description.Contains(term) ||
                    (c.Charity != null && c.Charity.Name.Contains(term)));
            }

            var total = await q.CountAsync();

            var rows = await q
                .OrderBy(c => c.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new
                {
                    c.Title,
                    c.Description,
                    c.ImageUrl,
                    c.GoalAmount,
                    c.RaisedAmount,
                    c.isCritical,
                    c.StartDate,
                    c.Duration,
                    c.Category,
                    c.ExternalId,
                    c.SupportingCharitiesJson
                })
                .ToListAsync();

            var dtos = rows.Select(c =>
            {
                IEnumerable<string> supporters = Array.Empty<string>();
                if (!string.IsNullOrWhiteSpace(c.SupportingCharitiesJson))
                {
                    try { supporters = JsonSerializer.Deserialize<IEnumerable<string>>(c.SupportingCharitiesJson!) ?? Array.Empty<string>(); }
                    catch { supporters = Array.Empty<string>(); }
                }

                return new CampaignScrapedDto
                {
                    Title = c.Title,
                    Description = c.Description,
                    ImageUrl = c.ImageUrl,
                    SupportingCharities = supporters,
                    GoalAmount = c.GoalAmount,
                    RaisedAmount = c.RaisedAmount,
                    IsCritical = c.isCritical,
                    StartDate = c.StartDate.ToString("yyyy-MM-dd"),
                    DurationDays = c.Duration,
                    Category = c.Category.ToString(),
                    ExternalId = c.ExternalId
                };
            });

            return Ok(new PaginatedResultDto<CampaignScrapedDto>
            {
                Items = dtos,
                Page = page,
                PageSize = pageSize,
                Total = total
            });
        }



    /// <summary>
    /// Bulk import campaigns (SuperAdmin only). Links to the first matching existing charity by name from supporting_charities or charity_name.
    /// Missing numeric/date fields will be randomized; duplicates (same Title+Charity) will be skipped.
    /// </summary>
    [HttpPost("import")]
    [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ImportCampaigns([FromBody] List<CampaignImportItemDto> items)
        {
            if (items == null || items.Count == 0)
                return BadRequest(new { error = "Empty payload" });

            int imported = 0, skipped = 0, withoutCharity = 0, duplicates = 0, invalid = 0;
            var errors = new List<object>();

            foreach (var i in items)
            {
                try
                {
                    // Basic validation
                    var title = (i.Title ?? string.Empty).Trim();
                    var description = (i.Description ?? string.Empty).Trim();
                    var imageUrl = (i.ImageUrl ?? string.Empty).Trim();

                    if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description) || string.IsNullOrWhiteSpace(imageUrl))
                    {
                        invalid++;
                        continue;
                    }

                    // Resolve charity by provided charity_name first, else supporting_charities
                    var charityNames = new List<string>();
                    if (!string.IsNullOrWhiteSpace(i.CharityName)) charityNames.Add(i.CharityName!.Trim());
                    if (i.SupportingCharities != null) charityNames.AddRange(i.SupportingCharities.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()));

                    Charity? charity = null;
                    foreach (var name in charityNames.Distinct())
                    {
                        var n = name;
                        charity = await _unitOfWork.Charity.GetAsync(c => c.Name == n || EF.Functions.Like(c.Name, $"%{n}%"));
                        if (charity != null) break;
                    }

                    if (charity == null)
                    {
                        // Fallback: assign randomly from existing 101 charities (current DB state)
                        var allCharities = await _unitOfWork.Charity.GetAllAsync();
                        if (allCharities == null || allCharities.Count == 0)
                        {
                            withoutCharity++;
                            continue;
                        }
                        charity = allCharities[Random.Shared.Next(0, allCharities.Count)];
                    }

                    // Check duplicate by Title+Charity (repository throws when not found, so handle gracefully)
                    Campaign? exists = null;
                    try
                    {
                        exists = await _unitOfWork.Campaign.GetAsync(c => c.Title == title && c.CharityID == charity.Id);
                    }
                    catch
                    {
                        exists = null; // not found -> safe to insert
                    }
                    if (exists != null)
                    {
                        duplicates++;
                        continue;
                    }

                    // Randomize missing values
                    var rng = Random.Shared;
                    var startDate = i.StartDate ?? DateTime.UtcNow.Date.AddDays(-rng.Next(0, 60));
                    var duration = i.DurationDays ?? rng.Next(20, 61); // 20-60 days
                    var goal = i.GoalAmount ?? Math.Round(rng.Next(50_000, 500_001) / 100.0) * 100; // round to nearest 100
                    var raised = i.RaisedAmount ?? Math.Round(goal * (rng.Next(20, 81) / 100.0), 2);
                    var isCritical = i.IsCritical ?? false;

                    // Infer category
                    var category = InferCategory(i.Category, title, description);

                    // Determine status
                    var status = CampainStatusEnum.inProgress;
                    if (raised >= goal) status = CampainStatusEnum.Completed;
                    else if (DateTime.UtcNow.Date > startDate.AddDays(duration)) status = CampainStatusEnum.expired;

                    // Properly resolve the FK id (Charity inherits from UserAccount, local Charity.Id may be hidden/0)
                    var charityUserId = (charity as UserAccount)?.Id ?? charity.Id;

                    var campaign = new Campaign
                    {
                        Title = title,
                        Description = description,
                        ImageUrl = imageUrl,
                        ExternalId = i.ExternalId,
                        SupportingCharitiesJson = i.SupportingCharities != null && i.SupportingCharities.Count > 0
                            ? JsonSerializer.Serialize(i.SupportingCharities)
                            : null,
                        StartDate = startDate,
                        Duration = duration,
                        GoalAmount = goal,
                        RaisedAmount = raised,
                        isCritical = isCritical,
                        Category = category,
                        Status = status,
                        CharityID = charityUserId
                    };

                    await _unitOfWork.Campaign.AddAsync(campaign);
                    imported++;
                }
                catch (Exception ex)
                {
                    skipped++;
                    errors.Add(new { title = i?.Title, error = ex.Message });
                }
            }
            try
            {
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                errors.Add(new { error = $"Save failed: {ex.Message}", inner = ex.InnerException?.Message });
                return StatusCode(500, new { imported, skipped, withoutCharity, duplicates, invalid, errors });
            }

            return Ok(new { imported, skipped, withoutCharity, duplicates, invalid, errors });
        }

        /// <summary>
        /// Import or update campaigns by Title. If a campaign with the same Title exists, it will be UPDATED in-place:
        /// - ExternalId and SupportingCharitiesJson are set from the payload
        /// - ImageUrl, Description optionally updated if provided
        /// - Numeric/date fields and category are updated only if provided to avoid breaking existing demo data
        /// If not found, it will be created using the same logic as ImportCampaigns (including charity resolution).
        /// Temporarily AllowAnonymous to ease dev automation; tighten to SuperAdmin later.
        /// </summary>
    [HttpPost("import-or-update")]
    [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ImportOrUpdate([FromBody] List<CampaignImportItemDto> items)
        {
            if (items == null || items.Count == 0)
                return BadRequest(new { error = "Empty payload" });

            int inserted = 0, updated = 0, skipped = 0, invalid = 0;
            var errors = new List<object>();

            foreach (var i in items)
            {
                try
                {
                    var title = (i.Title ?? string.Empty).Trim();
                    if (string.IsNullOrWhiteSpace(title)) { invalid++; continue; }

                    var existing = await _context.Campaigns.FirstOrDefaultAsync(c => c.Title == title);
                    if (existing != null)
                    {
                        // Update minimal fields and scraped metadata
                        if (!string.IsNullOrWhiteSpace(i.Description)) existing.Description = i.Description!.Trim();
                        if (!string.IsNullOrWhiteSpace(i.ImageUrl)) existing.ImageUrl = i.ImageUrl!.Trim();
                        if (i.GoalAmount.HasValue) existing.GoalAmount = i.GoalAmount.Value;
                        if (i.RaisedAmount.HasValue) existing.RaisedAmount = i.RaisedAmount.Value;
                        if (i.IsCritical.HasValue) existing.isCritical = i.IsCritical.Value;
                        if (i.StartDate.HasValue) existing.StartDate = i.StartDate.Value;
                        if (i.DurationDays.HasValue) existing.Duration = i.DurationDays.Value;

                        // Category update if provided
                        if (!string.IsNullOrWhiteSpace(i.Category))
                        {
                            existing.Category = InferCategory(i.Category, title, existing.Description);
                        }

                        existing.ExternalId = i.ExternalId;
                        existing.SupportingCharitiesJson = (i.SupportingCharities != null && i.SupportingCharities.Count > 0)
                            ? JsonSerializer.Serialize(i.SupportingCharities)
                            : existing.SupportingCharitiesJson; // keep old if not provided

                        updated++;
                        continue;
                    }

                    // Otherwise, create new using the same logic as ImportCampaigns
                    var description = (i.Description ?? string.Empty).Trim();
                    var imageUrl = (i.ImageUrl ?? string.Empty).Trim();
                    if (string.IsNullOrWhiteSpace(description) || string.IsNullOrWhiteSpace(imageUrl))
                    {
                        invalid++;
                        continue;
                    }

                    // Resolve charity by provided charity_name first, else supporting_charities
                    var charityNames = new List<string>();
                    if (!string.IsNullOrWhiteSpace(i.CharityName)) charityNames.Add(i.CharityName!.Trim());
                    if (i.SupportingCharities != null) charityNames.AddRange(i.SupportingCharities.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()));

                    Charity? charity = null;
                    foreach (var name in charityNames.Distinct())
                    {
                        var n = name;
                        charity = await _unitOfWork.Charity.GetAsync(c => c.Name == n || EF.Functions.Like(c.Name, $"%{n}%"));
                        if (charity != null) break;
                    }

                    if (charity == null)
                    {
                        var allCharities = await _unitOfWork.Charity.GetAllAsync();
                        if (allCharities == null || allCharities.Count == 0)
                        {
                            skipped++;
                            errors.Add(new { title, error = "No charities available to assign" });
                            continue;
                        }
                        charity = allCharities[Random.Shared.Next(0, allCharities.Count)];
                    }

                    var rng = Random.Shared;
                    var startDate = i.StartDate ?? DateTime.UtcNow.Date.AddDays(-rng.Next(0, 60));
                    var duration = i.DurationDays ?? rng.Next(20, 61);
                    var goal = i.GoalAmount ?? Math.Round(rng.Next(50_000, 500_001) / 100.0) * 100;
                    var raised = i.RaisedAmount ?? Math.Round(goal * (rng.Next(20, 81) / 100.0), 2);
                    var isCritical = i.IsCritical ?? false;
                    var category = InferCategory(i.Category, title, description);

                    var status = CampainStatusEnum.inProgress;
                    if (raised >= goal) status = CampainStatusEnum.Completed;
                    else if (DateTime.UtcNow.Date > startDate.AddDays(duration)) status = CampainStatusEnum.expired;

                    var charityUserId = (charity as UserAccount)?.Id ?? charity.Id;

                    var campaign = new Campaign
                    {
                        Title = title,
                        Description = description,
                        ImageUrl = imageUrl,
                        ExternalId = i.ExternalId,
                        SupportingCharitiesJson = i.SupportingCharities != null && i.SupportingCharities.Count > 0
                            ? JsonSerializer.Serialize(i.SupportingCharities)
                            : null,
                        StartDate = startDate,
                        Duration = duration,
                        GoalAmount = goal,
                        RaisedAmount = raised,
                        isCritical = isCritical,
                        Category = category,
                        Status = status,
                        CharityID = charityUserId
                    };

                    await _unitOfWork.Campaign.AddAsync(campaign);
                    inserted++;
                }
                catch (Exception ex)
                {
                    skipped++;
                    errors.Add(new { title = i?.Title, error = ex.Message });
                }
            }

            try
            {
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                errors.Add(new { error = $"Save failed: {ex.Message}", inner = ex.InnerException?.Message });
                return StatusCode(500, new { inserted, updated, skipped, invalid, errors });
            }

            return Ok(new { inserted, updated, skipped, invalid, errors });
        }

        private static CampaignCategoryEnum InferCategory(string? provided, string title, string description)
        {
            if (!string.IsNullOrWhiteSpace(provided))
            {
                var p = provided.Trim().ToLowerInvariant();
                if (p.Contains("health") || p.Contains("صحة") || p.Contains("طبي")) return CampaignCategoryEnum.Health;
                if (p.Contains("education") || p.Contains("تعليم")) return CampaignCategoryEnum.Education;
                if (p.Contains("food") || p.Contains("غذاء") || p.Contains("طعام") || p.Contains("سلة") || p.Contains("إطعام")) return CampaignCategoryEnum.Food;
                if (p.Contains("shelter") || p.Contains("مأوى") || p.Contains("سكن")) return CampaignCategoryEnum.Shelter;
                if (p.Contains("يتيم") || p.Contains("الأيتام") || p.Contains("orphans")) return CampaignCategoryEnum.Orphans;
            }

            var t = $"{title} {description}".ToLowerInvariant();
            if (t.Contains("صحة") || t.Contains("علاج") || t.Contains("طبي") || t.Contains("دواء") || t.Contains("مريض")) return CampaignCategoryEnum.Health;
            if (t.Contains("تعليم") || t.Contains("مدرسة") || t.Contains("طلاب") || t.Contains("جامع")) return CampaignCategoryEnum.Education;
            if (t.Contains("غذاء") || t.Contains("طعام") || t.Contains("سلة") || t.Contains("إطعام")) return CampaignCategoryEnum.Food;
            if (t.Contains("مأوى") || t.Contains("سكن") || t.Contains("الفقراء") || t.Contains("خيمة") || t.Contains("لاجئ")) return CampaignCategoryEnum.Shelter;
            if (t.Contains("يتيم") || t.Contains("أيتام") || t.Contains("كفالة")) return CampaignCategoryEnum.Orphans;
            if (t.Contains("إغاثة") || t.Contains("اغاثة") || t.Contains("الإغاثة العاجلة")) return CampaignCategoryEnum.Other; // could be a separate enum later
            return CampaignCategoryEnum.Other;
        }

        // (DELETE) /api/Campaign/{id}
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _unitOfWork.Campaign.GetAsync(id);
            if (existing == null) return NotFound("Campaign not found.");

            await _unitOfWork.Campaign.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        // (DELETE) /api/Campaign/by-title?title=...
    [HttpDelete("by-title")]
    [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteByTitle([FromQuery] string title)
        {
            if (string.IsNullOrWhiteSpace(title)) return BadRequest("Title is required.");
            Campaign? existing = null;
            try
            {
                existing = await _unitOfWork.Campaign.GetAsync(c => c.Title == title.Trim());
            }
            catch
            {
                existing = null;
            }

            if (existing == null) return NotFound("Campaign not found.");

            await _unitOfWork.Campaign.DeleteAsync(existing.Id);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

    }
}
