using AtharPlatform.DTOs;
using AtharPlatform.Dtos;
using AtharPlatform.Models;
using AtharPlatform.Models.Enum;
using AtharPlatform.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace AtharPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignService _service;
        private readonly Context _context;
        private readonly IWebHostEnvironment _env;

        public CampaignController(ICampaignService service, Context context, IWebHostEnvironment env)
        {
            _service = service;
            _context = context;
            _env = env;
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
// this for scrapping
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>

        // GET /api/Campaign/GetAll?query=&page=1&pageSize=12
        // Returns paginated campaigns. If a campaign has supporting_charities it will be returned in supporting_charities (and charity_name will be omitted), otherwise charity_name is returned.
        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedResultDto<CampaignDto>>> GetAll([FromQuery] string? query, [FromQuery] int page = 1, [FromQuery] int pageSize = 12)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest("Page and PageSize must be greater than zero.");

            var q = _context.Campaigns.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                var term = query.Trim();
                q = q.Where(c => c.Title.Contains(term) || c.Description.Contains(term) || (c.Charity != null && c.Charity.Name.Contains(term)));
            }

            var total = await q.CountAsync();

            var rows = await q
                .OrderBy(c => c.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new
                {
                    c.Id,
                    c.Title,
                    c.Description,
                    Image = c.ImageUrl,
                    c.GoalAmount,
                    c.RaisedAmount,
                    c.StartDate,
                    EndDate = c.EndDate,
                    c.Category,
                    c.isCritical,
                    CharityName = c.Charity != null ? c.Charity.Name : null,
                    c.SupportingCharitiesJson
                })
                .ToListAsync();

            var items = rows.Select(r =>
            {
                IEnumerable<string>? supporters = null;
                if (!string.IsNullOrWhiteSpace(r.SupportingCharitiesJson))
                {
                    try { supporters = JsonSerializer.Deserialize<IEnumerable<string>>(r.SupportingCharitiesJson!); }
                    catch { supporters = null; }
                }

                return new CampaignDto
                {
                    Id = r.Id,
                    Title = r.Title,
                    Description = r.Description,
                    Image = r.Image,
                    GoalAmount = r.GoalAmount,
                    RaisedAmount = r.RaisedAmount,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    Category = r.Category,
                    IsCritical = r.isCritical,
                    SupportingCharities = supporters,
                    CharityName = (supporters == null || !supporters.Any()) ? r.CharityName : null
                };
            }).ToList();

            return Ok(new PaginatedResultDto<CampaignDto>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                Total = total
            });
        }

        // GET /api/Campaign/scraped - returns the scraped shape (snake_case)
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
                q = q.Where(c => c.Title.Contains(term) || c.Description.Contains(term) || (c.Charity != null && c.Charity.Name.Contains(term)));
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
                    ImageUrl = c.ImageUrl,
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

        // (scraped-file endpoint removed)

        // Other endpoints delegate to the service
        
    }
}