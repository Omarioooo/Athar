using AtharPlatform.Dtos;
using AtharPlatform.DTOs;
using AtharPlatform.Models.Enum;
using AtharPlatform.Repositories;
using AtharPlatform.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;


namespace AtharPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignService _campaignService;
        private readonly IUnitOfWork _unitOfWork;

        public CampaignController(ICampaignService service, IUnitOfWork unitOfWork)
        {
            _campaignService = service;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<AtharPlatform.Dtos.PaginatedResultDto<CampaignDto>>>
            GetAll(
                [FromQuery] int page = 1, 
                [FromQuery] int pageSize = 12,
                [FromQuery] CampainStatusEnum? status = null,
                [FromQuery] CampaignCategoryEnum? category = null,
                [FromQuery] string? search = null,
                [FromQuery] bool? isCritical = null,
                [FromQuery] double? minGoalAmount = null,
                [FromQuery] double? maxGoalAmount = null,
                [FromQuery] DateTime? startDateFrom = null,
                [FromQuery] DateTime? startDateTo = null,
                [FromQuery] int? charityId = null)
        {
            try
            {
                var campaigns = await _campaignService.GetPaginatedAsync(
                    page, 
                    pageSize, 
                    status,
                    category,
                    search,
                    isCritical,
                    minGoalAmount,
                    maxGoalAmount,
                    startDateFrom,
                    startDateTo,
                    charityId,
                    inCludeCharity: true);

                var total = await _campaignService.GetCountOfCampaignsAsync(
                    status,
                    category,
                    search,
                    isCritical,
                    minGoalAmount,
                    maxGoalAmount,
                    startDateFrom,
                    startDateTo,
                    charityId);

                var result = new AtharPlatform.Dtos.PaginatedResultDto<CampaignDto>
                {
                    Items = campaigns,
                    Page = page,
                    PageSize = pageSize,
                    Total = total
                };

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred during fetching campaigns." });
            }
        }

        [HttpPost("import")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Import([FromBody] IEnumerable<CampaignImportItemDto> items)
        {
            if (items == null) return BadRequest(new { message = "No data provided" });

            var now = DateTime.UtcNow;
            var added = 0;
            foreach (var i in items)
            {
                if (string.IsNullOrWhiteSpace(i?.Title) || string.IsNullOrWhiteSpace(i?.Description))
                    continue;

                int charityId = 0;
                string? nameHint = i.CharityName;
                if (string.IsNullOrWhiteSpace(nameHint) && (i.SupportingCharities?.Any() ?? false))
                    nameHint = i.SupportingCharities!.FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(nameHint))
                {
                    var nm = nameHint.Trim();
                    try
                    {
                        // Only link against imported charities
                        var charity = await _unitOfWork.Charities.GetWithExpressionAsync(c => c.IsScraped && (c.Name == nm || c.Name.Contains(nm)));
                        if (charity != null) charityId = charity.Id;
                    }
                    catch { /* skip if not found */ }
                }

                if (charityId == 0) continue; // skip campaigns that we can't link

                var cat = CampaignCategoryEnum.Other;
                if (!string.IsNullOrWhiteSpace(i.Category) && Enum.TryParse<CampaignCategoryEnum>(i.Category, true, out var parsed))
                    cat = parsed;

                // Validate scraped campaigns have ImageUrl (Image should be null for scraped data)
                if (string.IsNullOrWhiteSpace(i.ImageUrl))
                    continue; // Skip campaigns without image URL

                var campaign = new Campaign
                {
                    Title = i.Title!.Trim(),
                    Description = i.Description!.Trim(),
                    Image = null, // Scraped campaigns should NOT have binary image data
                    ImageUrl = i.ImageUrl.Trim(),
                    isCritical = i.IsCritical ?? false,
                    StartDate = i.StartDate ?? now,
                    Duration = i.DurationDays ?? 30,
                    GoalAmount = i.GoalAmount ?? 0,
                    RaisedAmount = i.RaisedAmount ?? 0,
                    IsInKindDonation = false,
                    Category = cat,
                    Status = CampainStatusEnum.inProgress,
                    CharityID = charityId,
                    ExternalId = i.ExternalId,
                    SupportingCharitiesJson = (i.SupportingCharities != null && i.SupportingCharities.Any()) ? JsonSerializer.Serialize(i.SupportingCharities) : null
                };

                await _unitOfWork.Campaigns.AddAsync(campaign);
                added++;
            }

            await _unitOfWork.SaveAsync();
            return Ok(new { imported = added });
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<List<CampaignDto>>> Search([FromQuery] string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return BadRequest(new { message = "Search keyword cannot be empty." });

                var results = await _campaignService.SearchAsync(keyword, inCludeCharity: true);

                if (results == null || !results.Any())
                    return NotFound(new { message = $"No campaigns found matching '{keyword}'." });

                return Ok(results);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred during campaign search." });
            }
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetCampaign(int id, [FromQuery] bool inProgress = true)
        {
            try
            {
                var campaign = await _campaignService.GetAsync(id, inProgress: inProgress, inCludeCharity: true);

                return Ok(campaign);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred during campaign search." });
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetByType(CampaignCategoryEnum type)
        {
            try
            {
                var result = await _campaignService.GetByTypeAsync(type, inCludeCharity: true);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred during campaign search." });
            }
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllTypes()
        {
            try
            {
                var result = await _campaignService.GetAllTypesAsync();
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred during campaign search." });
            }
        }



        [HttpPost("[action]")]
        public async Task<IActionResult> CreateCampaign(AddCampaignDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var isCreated = await _campaignService.CreateAsync(model);
                if (!isCreated)
                    return BadRequest(new { message = "Failed to create campaign. Please try again later." });

                return Ok(new { message = "Campaign created successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred during campaign search." });
            }
        }

        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> UpdateCampaign(UpdatCampaignDto model, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedCampaign = await _campaignService.UpdateAsync(model);

                if (updatedCampaign == null)
                    return BadRequest(new { message = "Failed to update campaign. Please try again later." });

                return Ok(new { message = "Campaign updated successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred during campaign search." });
            }
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteCampaign([FromRoute] int id)
        {
            try
            {
                var isDeleted = await _campaignService.DeleteAsync(id);

                if (!isDeleted)
                    return BadRequest(new { message = "Failed to delete campaign. Please try again later." });

                return Ok(new { message = "Campaign deleted successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred during campaign search." });
            }
        }
    }
}