using AtharPlatform.Dtos;
using AtharPlatform.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                    MegaKheirUrl = c.ScrapedInfo != null ? c.ScrapedInfo.MegaKheirUrl : null,
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
                MegaKheirUrl = c.ScrapedInfo != null ? c.ScrapedInfo.MegaKheirUrl : null,
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
            // Case-insensitive match without ToLower by using a CI collation
            Charity? c = null;
            try
            {
                c = await _uow.Charity.GetAsync(x => EF.Functions.Collate(x.Name, "SQL_Latin1_General_CP1_CI_AS") == trimmed);
            }
            catch
            {
                c = null;
            }
            if (c == null) return NotFound("Charity not found.");

            var dto = new CharityCardDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Image = c.Image,
                ImageUrl = c.ScrapedInfo != null ? c.ScrapedInfo.ImageUrl : null,
                ExternalWebsiteUrl = c.ScrapedInfo != null ? c.ScrapedInfo.ExternalWebsiteUrl : null,
                MegaKheirUrl = c.ScrapedInfo != null ? c.ScrapedInfo.MegaKheirUrl : null
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
                MegaKheirUrl = c.ScrapedInfo != null ? c.ScrapedInfo.MegaKheirUrl : null,
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

                if (i.ImageUrl != null || i.ExternalWebsiteUrl != null || i.MegaKheirUrl != null)
                {
                    charity.ScrapedInfo = new Models.CharityExternalInfo
                    {
                        ImageUrl = i.ImageUrl,
                        ExternalWebsiteUrl = i.ExternalWebsiteUrl,
                        MegaKheirUrl = i.MegaKheirUrl
                    };
                }

                entities.Add(charity);
            }

            await _uow.Charity.BulkImportAsync(entities);
            await _uow.SaveAsync();
            return Ok(new { imported = entities.Count });
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
