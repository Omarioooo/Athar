using AtharPlatform.Dtos;
using AtharPlatform.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AtharPlatform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharitiesController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public CharitiesController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // (GET) /api/charities?query=&page=1&pageSize=12
        [HttpGet]
        public async Task<ActionResult<PaginatedResultDto<CharityListDto>>> GetAll([FromQuery] string? query, [FromQuery] int page = 1, [FromQuery] int pageSize = 12)
        {
            var total = await _uow.Charity.CountAsync(query);
            var items = await _uow.Charity.GetPageAsync(query, page, pageSize);

            var dto = new PaginatedResultDto<CharityListDto>
            {
                Items = items.Select(c => new CharityListDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Image = c.Image,
                    ImageUrl = c.ImageUrl,
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
        public async Task<ActionResult<CharityDetailDto>> GetById(int id)
        {
            var c = await _uow.Charity.GetWithCampaignsAsync(id);
            if (c == null) return NotFound("Charity not found.");

            var dto = new CharityDetailDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Image = c.Image,
                ImageUrl = c.ImageUrl,
                ExternalWebsiteUrl = c.ExternalWebsiteUrl,
                MegaKheirUrl = c.MegaKheirUrl,
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

        // (GET) /api/charities/search?query=
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<CharityListDto>>> Search([FromQuery] string query, [FromQuery] int take = 10)
        {
            if (string.IsNullOrWhiteSpace(query)) return Ok(Array.Empty<CharityListDto>());
            var items = await _uow.Charity.GetPageAsync(query, 1, take);
            return Ok(items.Select(c => new CharityListDto
            {
                Id = c.Id,
                Name = c.Name,
                Image = c.Image,
                ImageUrl = c.ImageUrl,
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
            var entities = items.Where(i => !string.IsNullOrWhiteSpace(i.Name)).Select(i => new Models.Charity
            {
                Name = i.Name.Trim(),
                Description = i.Description ?? string.Empty,
                Image = i.Image, // if provided as bytes/base64 decoded by client
                ImageUrl = i.ImageUrl,
                ExternalWebsiteUrl = i.ExternalWebsiteUrl,
                MegaKheirUrl = i.MegaKheirUrl,
                IsScraped = true,
                ExternalId = i.ExternalId,
                ImportedAt = now
            }).ToList();

            await _uow.Charity.BulkImportAsync(entities);
            await _uow.SaveAsync();
            return Ok(new { imported = entities.Count });
        }
    }
}
