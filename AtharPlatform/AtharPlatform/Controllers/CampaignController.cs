using AtharPlatform.Dtos;
using AtharPlatform.DTOs;
using AtharPlatform.Models;
using AtharPlatform.Models.Enum;
using AtharPlatform.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;
using static System.Net.Mime.MediaTypeNames;


namespace AtharPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : Controller
    {
        private readonly ICampaignRepository _icampaignrepository;
        private readonly IUnitOfWork _iunitofwork;
        public CampaignController(ICampaignRepository icampaignrepository, IUnitOfWork iunitofwork)
        {
            _icampaignrepository = icampaignrepository;
            _iunitofwork = iunitofwork;
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllgCampaign()
        {
            var campaign = await _icampaignrepository.GetAllAsync();
            var result = campaign.Select(c => new CampaignDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Image = c.Image,
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
        public async Task<IActionResult> GetCampaignById(int id)
        {
            var campaign = await _icampaignrepository.GetAsync(id);
            if (campaign == null)
                return NotFound("Campaign not found.");
            var result = new CampaignDto
            {
                Id = campaign.Id,
                Title = campaign.Title,
                Description = campaign.Description,
                Image = campaign.Image,
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
                Image = c.Image,
                GoalAmount = c.GoalAmount,
                RaisedAmount = c.RaisedAmount,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                Category = c.Category,
                CharityName = c.Charity.Name


            });
            return Ok(result);



        }
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateCampaign(AddCampaignDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var campaign = new Campaign
            {
                Title = model.Title,
                Description = model.Description,
                Image = model.Image,
                isCritical = model.IsCritical,
                StartDate = model.StartDate ?? DateTime.UtcNow,
                Duration = model.Duration,
                GoalAmount = model.GoalAmount,
                IsInKindDonation = model.IsInKindDonation,
                Category = model.Category,
                CharityID = model.CharityID,
                RaisedAmount = 0,
                Status = CampainStatusEnum.inProgress
            };

            await _icampaignrepository.AddAsync(campaign);
            await _iunitofwork.SaveAsync();
            var campaignView = new CampaignDto
            {
                Id = campaign.Id,
                Title = campaign.Title,
                Description = campaign.Description,
                Image = campaign.Image,
                GoalAmount = campaign.GoalAmount,
                RaisedAmount = campaign.RaisedAmount,
                StartDate = campaign.StartDate,
                EndDate = campaign.EndDate,
                Category = campaign.Category,
                CharityName = campaign.Charity?.Name
            };

            return CreatedAtAction(nameof(GetCampaignById), new { id = campaign.Id }, campaignView);
        }
        [HttpPut("[action]")]
        public async Task<IActionResult>UpdateCampaign(UpdatCampaignDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var campaign = await _icampaignrepository.GetAsync(model.Id);
            if (campaign == null)
            {
                return NotFound("Campaign not found.");
            }
            campaign.Title = model.Title;
            campaign.Description = model.Description;
            campaign.Image = model.Image;
            campaign.isCritical = model.IsCritical;
            campaign.Duration = model.Duration;
            campaign.GoalAmount = model.GoalAmount;
            campaign.IsInKindDonation = model.IsInKindDonation;
            campaign.Category = model.Category;
           _icampaignrepository.Update(campaign);
           await  _iunitofwork.SaveAsync();
            return Ok(new { message = "Campaign updated successfully" });

        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteCampaign(int id)
        {
            
            var campaign = await _icampaignrepository.GetAsync(id);
            if (campaign == null)
                return NotFound("Campaign not found.");

           
            await _icampaignrepository.DeleteAsync(id);
            await _iunitofwork.SaveAsync();

            return Ok(new { message = "Campaign deleted successfully" });
        }


    }
}
