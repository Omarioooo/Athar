using AtharPlatform.DTOs;
using AtharPlatform.Models;
using AtharPlatform.Models.Enum;
using AtharPlatform.Services;
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
        private readonly ICampaignService _service;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Context _context;

        public CampaignController(ICampaignService service, IUnitOfWork unitOfWork, Context context)
        {
            _service = service;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        // =====================================
        // ========== Service-based =============
        // =====================================

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllgCampaigntousers()
        {
            try
            {
                var result = await _service.GetAllAsyncforusers();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllgCampaign()
        {
            try
            {
                var result = await _service.GetAllAsyncforadmin();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetCampaignById(int id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetCampaignByIdtousers(int id)
        {
            try
            {
                var result = await _service.GetByIdAsynctousers(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetByType(CampaignCategoryEnum type)
        {
            try
            {
                var result = await _service.GetByTypeAsync(type);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetByTypetousers(CampaignCategoryEnum type)
        {
            try
            {
                var result = await _service.GetByTypeAsynctousers(type);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> SearchCampaigns(string keyword)
        {
            try
            {
                var result = await _service.SearchAsync(keyword);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetPaginated(int page = 1, int pageSize = 10)
        {
            try
            {
                var result = await _service.GetPaginatedAsync(page, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateCampaign(AddCampaignDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _service.CreateAsync(model);
                return CreatedAtAction(nameof(GetCampaignById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateCampaign(UpdatCampaignDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedCampaign = await _service.UpdateAsync(model);
                return Ok(updatedCampaign);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteCampaign(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok(new { message = "Campaign deleted successfully" });
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        // =====================================
        // ========== UnitOfWork-based ==========
        // =====================================

        // GET /api/Campaign/scraped
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

        // POST /api/Campaign/import
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
                    var title = (i.Title ?? string.Empty).Trim();
                    var description = (i.Description ?? string.Empty).Trim();
                    var imageUrl = (i.ImageUrl ?? string.Empty).Trim();

                    if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description) || string.IsNullOrWhiteSpace(imageUrl))
                    {
                        invalid++;
                        continue;
                    }

                    // Resolve charity
                    var charityNames = new List<string>();
                    if (!string.IsNullOrWhiteSpace(i.CharityName)) charityNames.Add(i.CharityName!.Trim());
                    if (i.SupportingCharities != null) charityNames.AddRange(i.SupportingCharities.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()));

                    Charity? charity = null;
                    foreach (var name in charityNames.Distinct())
                    {
                        charity = await _unitOfWork.Charity.GetAsync(c => c.Name == name || EF.Functions.Like(c.Name, $"%{name}%"));
                        if (charity != null) break;
                    }

                    if (charity == null)
                    {
                        var allCharities = await _unitOfWork.Charity.GetAllAsync();
                        if (allCharities == null || allCharities.Count == 0)
                        {
                            withoutCharity++;
                            continue;
                        }
                        charity = allCharities[Random.Shared.Next(0, allCharities.Count)];
                    }

                    Campaign? exists = null;
                    try
                    {
                        exists = await _unitOfWork.Campaign.GetAsync(c => c.Title == title && c.CharityID == charity.Id);
                    }
                    catch { exists = null; }

                    if (exists != null)
                    {
                        duplicates++;
                        continue;
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
            if (t.Contains("إغاثة") || t.Contains("اغاثة")) return CampaignCategoryEnum.Other;

            return CampaignCategoryEnum.Other;
        }
    }
}
