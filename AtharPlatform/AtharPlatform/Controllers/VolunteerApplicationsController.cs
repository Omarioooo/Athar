using AtharPlatform.DTOs;
using AtharPlatform.Models;
using AtharPlatform.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AtharPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolunteerApplicationsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public VolunteerApplicationsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get all applications for a specific campaign
        /// </summary>
        [HttpGet("by-campaign/{campaignId}")]
        public async Task<IActionResult> GetByCampaign(int campaignId)
        {
            if (campaignId <= 0)
                return BadRequest(new { Message = "Invalid campaign ID." });

            try
            {
                var applications = await _unitOfWork.VolunteerApplications.GetByCampaignAsync(campaignId);

                if (applications == null || !applications.Any())
                    return NotFound(new { Message = "No volunteer applications found for this campaign." });

                return Ok(applications.Select(MapToDTO).ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching applications.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get application by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Invalid application ID." });

            try
            {
                var application = await _unitOfWork.VolunteerApplications.GetAsync(id);

                if (application == null)
                    return NotFound(new { Message = "Application not found." });

                return Ok(MapToDTO(application));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching the application.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new volunteer application
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VolunteerApplicationDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.Age <= 0)
                return BadRequest(new { Message = "Age must be greater than 0." });

            try
            {
                var application = new VolunteerApplication
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Age = dto.Age,
                    PhoneNumber = dto.PhoneNumber,
                    Country = dto.Country,
                    City = dto.City,
                    IsFirstTime = dto.IsFirstTime,
                    CharityVolunteerId = dto.CharityVolunteerId
                };

                await _unitOfWork.VolunteerApplications.AddAsync(application);
                await _unitOfWork.SaveAsync();

                return CreatedAtAction(nameof(GetById), new { id = application.Id }, MapToDTO(application));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the application.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Update application status (IsFirstTime)
        /// </summary>
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] bool isFirstTime)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Invalid application ID." });

            try
            {
                var application = await _unitOfWork.VolunteerApplications.GetAsync(id);

                if (application == null)
                    return NotFound(new { Message = "Application not found." });

                application.IsFirstTime = isFirstTime;
                await _unitOfWork.VolunteerApplications.UpdateAsync(application);
                await _unitOfWork.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the application status.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Delete an application
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Invalid application ID." });

            try
            {
                var application = await _unitOfWork.VolunteerApplications.GetAsync(id);

                if (application == null)
                    return NotFound(new { Message = "Application not found." });

                await _unitOfWork.VolunteerApplications.DeleteAsync(id);
                await _unitOfWork.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the application.", Error = ex.Message });
            }
        }

        // Manual DTO mapping method
        private VolunteerApplicationDTO MapToDTO(VolunteerApplication a) => new VolunteerApplicationDTO
        {
            Id = a.Id,
            FirstName = a.FirstName,
            LastName = a.LastName,
            Age = a.Age,
            PhoneNumber = a.PhoneNumber,
            Country = a.Country,
            City = a.City,
            IsFirstTime = a.IsFirstTime,
            CharityVolunteerId = a.CharityVolunteerId
        };
    }
}
