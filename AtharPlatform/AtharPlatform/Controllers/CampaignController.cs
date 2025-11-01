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
<<<<<<< HEAD
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
=======
        private readonly IUnitOfWork _unitOfWork;

        public CampaignController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

            var total = await _unitOfWork.Campaign.CountAsync(query);

            if (total == 0)
            {
                return Ok(new PaginatedResultDto<CampaignDto>
                {
                    Items = new List<CampaignDto>(),
                    Page = page,
                    PageSize = pageSize,
                    Total = 0
                });
            }

            var campaigns = await _unitOfWork.Campaign.GetPageAsync(query, page, pageSize);

            var dto = new PaginatedResultDto<CampaignDto>
            {
                Items = campaigns.Select(c => new CampaignDto
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
                }),
                Page = page,
                PageSize = pageSize,
                Total = total
            };

            return Ok(dto);
        }


>>>>>>> master
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllgCampaign()
        {
<<<<<<< HEAD
            try
            {
                var result = await _service.GetAllAsyncforadmin();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
=======
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

>>>>>>> master
        }
//this for super admin
        [HttpGet("[action]")]
<<<<<<< HEAD
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
=======
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
                CharityName = c.Charity.Name
            });
            return Ok(result);
>>>>>>> master
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
                CharityName = c.Charity?.Name
            });

            return Ok(result);
        }




    }
}
