using AtharPlatform.Dtos;
using AtharPlatform.Models.Enum;
using AtharPlatform.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace AtharPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : Controller
    {
        private readonly ICampaignRepository _icampaignrepository;
        public CampaignController(ICampaignRepository icampaignrepository)
        {
            _icampaignrepository = icampaignrepository;
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllCampaign()
        {
            var campaign = await _icampaignrepository.GetAllAsync();
            var result = campaign.Select(c => new CampaignDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                ImageUrl = c.Image,
                GoalAmount = c.GoalAmount,
                RaisedAmount = c.RaisedAmount,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                Category = c.Category,
                CharityName = c.Charity.Name


            });
            return Ok(result);

        }
        [HttpGet("[action]")]
        public async Task<IActionResult>GetCampaignById(int id)
        {
            var campaign = await _icampaignrepository.GetAsync(id);
            if (campaign == null)
                return NotFound("Campaign not found.");
            var result = new CampaignDto
            {
                Id = campaign.Id,
                Title = campaign.Title,
                Description = campaign.Description,
                ImageUrl = campaign.Image,
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
            var campaign = await _icampaignrepository.GetByType(type);
            if (campaign == null)
                return NotFound("Campaign not found.");
            var result = campaign.Select(c => new CampaignDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                ImageUrl = c.Image,
                GoalAmount = c.GoalAmount,
                RaisedAmount = c.RaisedAmount,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                Category = c.Category,
                CharityName = c.Charity.Name


            });
            return Ok(result);
            

        }
    }
}
