using AtharPlatform.DTOs;
using AtharPlatform.Models.Enum;
using AtharPlatform.Services;
using Microsoft.AspNetCore.Mvc;

namespace AtharPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignService _service;

        public CampaignController(ICampaignService service)
        {
            _service = service;
        }
        //this for users
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
        //this for super admin
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
//this for super admin
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
        //this for users
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
        //this for super admins
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
        //this for users
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
        public async Task<IActionResult> GetAllTypes()
        {
            try
            {
                var result = await _service.GetAllTypesAsync();
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
        // all create and update and delete for admin of charities
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
    }
}
