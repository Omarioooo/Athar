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

        public CampaignController(ICampaignService service, IUnitOfWork unitOfWork)
        {
            _campaignService = service;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<PaginatedResultDto<CampaignDto>>>
            GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 12)
        {
            try
            {
                var campaigns = await _campaignService.GetPaginatedAsync(page, pageSize, inCludeCharity: true);

                var result = new PaginatedResultDto<CampaignDto>
                {
                    Items = campaigns,
                    Page = page,
                    PageSize = pageSize,
                    Total = await _campaignService.GetCountOfCampaignsAsync()
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