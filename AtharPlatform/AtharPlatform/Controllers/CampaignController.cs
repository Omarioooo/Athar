using System.Text.Json;
using AtharPlatform.Dtos;
using AtharPlatform.DTOs;
using AtharPlatform.Models.Enum;
using AtharPlatform.Repositories;
using AtharPlatform.Services;
using Microsoft.AspNetCore.Mvc;


namespace AtharPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignService _campaignService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountContextService _accountContextService;

        private readonly IWebHostEnvironment _env;


        public CampaignController(ICampaignService service, IUnitOfWork unitOfWork, IAccountContextService accountContextService, IWebHostEnvironment env)
        {
            _campaignService = service;
            _unitOfWork = unitOfWork;
            _accountContextService = accountContextService;
            _env = env;
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

        #region OldGetAll 
        //    [HttpGet("[action]")]
        //    public async Task<ActionResult<AtharPlatform.Dtos.PaginatedResultDto<CampaignDto>>>
        //GetAll(
        //    [FromQuery] int page = 1,
        //    [FromQuery] int pageSize = 12,
        //    [FromQuery] CampainStatusEnum? status = null,
        //    [FromQuery] CampaignCategoryEnum? category = null,
        //    [FromQuery] string? search = null,
        //    [FromQuery] bool? isCritical = null,
        //    [FromQuery] double? minGoalAmount = null,
        //    [FromQuery] double? maxGoalAmount = null,
        //    [FromQuery] DateTime? startDateFrom = null,
        //    [FromQuery] DateTime? startDateTo = null,
        //    [FromQuery] int? charityId = null)
        //    {
        //        try
        //        {
        //            var campaigns = await _campaignService.GetPaginatedAsync(
        //                page,
        //                pageSize,
        //                status,
        //                category,
        //                search,
        //                isCritical,
        //                minGoalAmount,
        //                maxGoalAmount,
        //                startDateFrom,
        //                startDateTo,
        //                charityId,
        //                inCludeCharity: true);

        //            var total = await _campaignService.GetCountOfCampaignsAsync(
        //                status,
        //                category,
        //                search,
        //                isCritical,
        //                minGoalAmount,
        //                maxGoalAmount,
        //                startDateFrom,
        //                startDateTo,
        //                charityId);

        //            Convert relative ImageUrls to full URLs
        //            foreach (var campaign in campaigns)
        //            {
        //                campaign.ImageUrl = ToFullUrl(campaign.ImageUrl);
        //            }

        //            var result = new AtharPlatform.Dtos.PaginatedResultDto<CampaignDto>
        //            {
        //                Items = campaigns,
        //                Page = page,
        //                PageSize = pageSize,
        //                Total = total
        //            };

        //            return Ok(result);
        //        }
        //        catch (ArgumentException ex)
        //        {
        //            return BadRequest(new { message = ex.Message });
        //        }
        //        catch (KeyNotFoundException ex)
        //        {
        //            return NotFound(new { message = ex.Message });
        //        }
        //        catch (InvalidOperationException ex)
        //        {
        //            return StatusCode(500, new { message = ex.Message });
        //        }
        //        catch (Exception)
        //        {
        //            return StatusCode(500, new { message = "An unexpected error occurred during fetching campaigns." });
        //        }
        //    }
        #endregion

        [HttpGet("[action]")]
        public async Task<ActionResult<PaginatedResultDto<CampaignDto>>> GetAll(
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
                var result = await _campaignService.GetPaginatedOptimizedAsync(
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
                    charityId
                );

                // Convert URLs directly (no loop on entity)
                foreach (var c in result.Items)
                    c.ImageUrl = ToFullUrl(c.ImageUrl);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

      

        [HttpPost("import")]
        //[Authorize(Roles = "Admin,SuperAdmin")]
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


        [HttpGet("GetCampaignsByCharityId/{charityId}")]
        public async Task<IActionResult> GetCampaignsByCharityId(int charityId)
        {
            try
            {
                var campaigns = await _campaignService.GetCampaignsByCharityIdAsync(charityId);

                if (campaigns == null || !campaigns.Any())
                    return NotFound(new { message = $"No campaigns found for CharityId {charityId}." });

                return Ok(campaigns);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching campaigns.", error = ex.Message });
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

        #region Old_Create

        // [HttpPost("[action]")]
        //// [Authorize(Roles = "CharityAdmin,SuperAdmin")]
        // public async Task<IActionResult> CreateCampaign([FromForm] AddCampaignDto model)
        // {
        //     if (!ModelState.IsValid)
        //         return BadRequest(ModelState);

        //     try
        //     {
        //         // If CharityAdmin, enforce ownership & charity approval
        //         if (User.IsInRole("CharityAdmin"))
        //         {
        //             var currentId = _accountContextService.GetCurrentAccountId();
        //             if (model.CharityID != currentId)
        //                 return Forbid();
        //             var charity = await _unitOfWork.Charities.GetAsync(currentId);
        //             if (charity == null || charity.Status != AtharPlatform.Models.Enums.CharityStatusEnum.Approved || !charity.IsActive)
        //                 return BadRequest(new { message = "Charity not approved or inactive." });
        //         }
        //         var isCreated = await _campaignService.CreateAsync(model);
        //         if (!isCreated)
        //             return BadRequest(new { message = "Failed to create campaign. Please try again later." });

        //         return Ok(new { message = "Campaign created successfully." });
        //     }
        //     catch (ArgumentException ex)
        //     {
        //         return BadRequest(new { message = ex.Message });
        //     }
        //     catch (KeyNotFoundException ex)
        //     {
        //         return NotFound(new { message = ex.Message });
        //     }
        //     catch (InvalidOperationException ex)
        //     {
        //         return StatusCode(500, new { message = ex.Message });
        //     }
        //     catch (Exception)
        //     {
        //         return StatusCode(500, new { message = "An unexpected error occurred during campaign search." });
        //     }
        // }
        #endregion

        [HttpPost("create/{id}")]
        public async Task<IActionResult> CreateCampaign(int id, [FromForm] CreateCampaignDto dto)
        {
            if (dto.ImageFile == null)
                return BadRequest("Campaign image is required.");

            // 1) Upload image
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.ImageFile.FileName)}";
            string folderPath = Path.Combine(_env.WebRootPath, "uploads/campaigns");
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.ImageFile.CopyToAsync(stream);
            }

            string imageUrl = $"https://localhost:5192/uploads/campaigns/{fileName}";

            // 2) Save campaign
            Campaign campaign = new()
            {
                Title = dto.Title,
                Description = dto.Description,
                GoalAmount = dto.GoalAmount,
                RaisedAmount = 0,
                Duration = dto.Duration,
                CharityID = id,
                Category = dto.Category,
                ImageUrl = imageUrl
            };

            await _unitOfWork.Campaigns.AddAsync(campaign);
            await _unitOfWork.SaveAsync();

            return Ok(new { message = "Campaign created successfully", imageUrl });
        }


        [HttpPut("[action]/{id}")]
        //[Authorize(Roles = "CharityAdmin,SuperAdmin")]
        public async Task<IActionResult> UpdateCampaign(UpdatCampaignDto model, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (User.IsInRole("CharityAdmin"))
                {
                    var currentId = _accountContextService.GetCurrentAccountId();
                    var existing = await _unitOfWork.Campaigns.GetAsync(id, includeCharity: false);
                    if (existing == null) return NotFound(new { message = "Campaign not found." });
                    if (existing.CharityID != currentId) return Forbid();
                }
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
        //[Authorize(Roles = "CharityAdmin,SuperAdmin")]
        public async Task<IActionResult> DeleteCampaign([FromRoute] int id)
        {
            try
            {
                if (User.IsInRole("CharityAdmin"))
                {
                    var currentId = _accountContextService.GetCurrentAccountId();
                    var existing = await _unitOfWork.Campaigns.GetAsync(id, includeCharity: false);
                    if (existing == null) return NotFound(new { message = "Campaign not found." });
                    if (existing.CharityID != currentId) return Forbid();
                }
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


        [HttpGet("Campaing_count_For_Charity/{charityId}")]
        public async Task<IActionResult> GetCharityCampaignCount(int charityId)
        {

            if (charityId <= 0)
                return BadRequest("Invalid Charity ID");

            // تحقق هل الجمعية موجودة
            var charityExists = await _unitOfWork.Charities.GetAll()
                .AnyAsync(c => c.Id == charityId );

            if (!charityExists)
                return NotFound("Charity Not Found");

            var result = await _campaignService.GetCountOfCampaignsByCharityIdAsync(charityId);
            return Ok(result);
        }

    }
}