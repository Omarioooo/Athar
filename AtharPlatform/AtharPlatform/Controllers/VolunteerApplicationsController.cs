using AtharPlatform.DTOs;
using AtharPlatform.Models;
using AtharPlatform.Repositories;
using AtharPlatform.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AtharPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class VolunteerApplicationsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVolunteerApplicationService _service;

        public VolunteerApplicationsController(IUnitOfWork unitOfWork, IVolunteerApplicationService service)
        {
            _unitOfWork = unitOfWork;
            _service = service;
        }

        /// <summary>
        /// Get application by ID
        /// </summary>
        [HttpGet("{id}")]
        //[Authorize(Roles = "CharityAdmin,SuperAdmin")]
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

        ///// <summary>
        ///// Apply for a volunteer opportunity
        ///// </summary>
        //[HttpPost("apply")]
        ////[Authorize(Roles = "Donor")] 
        //public async Task<IActionResult> Apply([FromBody] VolunteerApplicationDTO dto)
        //{
        //    //if (!ModelState.IsValid)
        //    //    return BadRequest(ModelState);

        //    //if (dto.Age < 11)
        //    //    return BadRequest(new { Message = "Age must be greater than 10." });

        //    //var volunteerOpportunity = await _unitOfWork.CharityVolunteers.GetByIdAsync(dto.CharityVolunteerId);

        //    //if (volunteerOpportunity == null)
        //    //    return NotFound(new { Message = "Volunteer opportunity not found." });


        //    //var existing = await _unitOfWork.VolunteerApplications
        //    //    .GetAllAsync();
        //    //if (existing.Any(a => a.Id == dto.Id && a.CharityVolunteerId == dto.CharityVolunteerId))
        //    //    return BadRequest(new { Message = "You have already applied for this opportunity." });

        //    //try
        //    //{
        //    //    var application = new VolunteerApplication
        //    //    {
        //    //        FirstName = dto.FirstName,
        //    //        LastName = dto.LastName,
        //    //        Age = dto.Age,
        //    //        PhoneNumber = dto.PhoneNumber,
        //    //        Country = dto.Country,
        //    //        City = dto.City,
        //    //        IsFirstTime = dto.IsFirstTime,
        //    //        CharityVolunteerId = dto.CharityVolunteerId
        //    //    };

        //    //    await _unitOfWork.VolunteerApplications.AddAsync(application);
        //    //    await _unitOfWork.SaveAsync();

        //    //    return CreatedAtAction(nameof(GetById), new { id = application.Id }, MapToDTO(application));
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    return StatusCode(500, new
        //    //    {
        //    //        Message = "Error creating application",
        //    //        Error = ex.InnerException?.Message ?? ex.Message
        //    //    });
        //    //}
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    if (dto.Age < 11)
        //        return BadRequest(new { Message = "Age must be greater than 10." });

        //    // تحقق من وجود Volunteer Slot
        //    var slot = await _unitOfWork.CharityVolunteers.GetSlotByCharityIdAsync(dto.CharityId);
        //    if (slot == null)
        //    {
        //        slot = new CharityVolunteer
        //        {
        //            CharityId = dto.CharityId,
        //            Date = DateTime.UtcNow
        //        };
        //        await _unitOfWork.CharityVolunteers.AddAsync(slot);
        //        await _unitOfWork.SaveAsync();
        //    }

        //    // تحقق من وجود طلب سابق لنفس المتطوع والفرصة
        //    var existing = await _unitOfWork.VolunteerApplications.GetAllAsync();
        //    if (existing.Any(a => a.Id == dto.Id && a.CharityVolunteerId == slot.Id))
        //        return BadRequest(new { Message = "You have already applied for this opportunity." });

        //    try
        //    {
        //        var application = new VolunteerApplication
        //        {
        //            FirstName = dto.FirstName,
        //            LastName = dto.LastName,
        //            Age = dto.Age,
        //            PhoneNumber = dto.PhoneNumber,
        //            Country = dto.Country,
        //            City = dto.City,
        //            IsFirstTime = dto.IsFirstTime,
        //            CharityVolunteerId = slot.Id
        //        };

        //        await _unitOfWork.VolunteerApplications.AddAsync(application);
        //        await _unitOfWork.SaveAsync();

        //        return CreatedAtAction(nameof(GetById), new { id = application.Id }, MapToDTO(application));
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new
        //        {
        //            Message = "Error creating application",
        //            Error = ex.InnerException?.Message ?? ex.Message
        //        });
        //    }
        //}



        ///// <summary>
        ///// Delete an application
        ///// </summary>
        //[HttpDelete("{id}")]
        ////[Authorize(Roles = "CharityAdmin,SuperAdmin")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    if (id <= 0)
        //        return BadRequest(new { Message = "Invalid application ID." });

        //    try
        //    {
        //        var application = await _unitOfWork.VolunteerApplications.GetAsync(id);

        //        if (application == null)
        //            return NotFound(new { Message = "Application not found." });

        //        await _unitOfWork.VolunteerApplications.DeleteAsync(id);
        //        await _unitOfWork.SaveAsync();

        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { Message = "An error occurred while deleting the application.", Error = ex.Message });
        //    }
        //}

        //[HttpGet("{id:int}/volunteer-opportunities_For_Charity")]
        ////[Authorize(Roles = "CharityAdmin,SuperAdmin")]
        //public async Task<IActionResult> GetVolunteerOpportunitiesByCharity(int id)
        //{
        //    var opportunities = await _unitOfWork.CharityVolunteers.GetByCharityIdAsync(id);

        //    if (opportunities == null || !opportunities.Any())
        //        return NotFound(new { Message = "No volunteer opportunities found for this charity." });


        //    var dtoList = opportunities.Select(o => new
        //    {
        //        o.CharityId,

        //        VolunteerApplications = o.VolunteerApplications.Select(va => new
        //        {
        //            va.Id,
        //            va.FirstName,
        //            va.LastName,
        //            va.Age,
        //            va.PhoneNumber,
        //            va.IsFirstTime


        //        })

        //    });

        //    return Ok(dtoList);
        //}
        [HttpPost("apply")]
        public async Task<IActionResult> Apply([FromBody] VolunteerApplicationDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _service.ApplyAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error creating application", Error = ex.Message });
            }
        }

        private VolunteerApplicationDTO MapToDTO(VolunteerApplication application)
        {
            return new VolunteerApplicationDTO
            {
                Id = application.Id,
                FirstName = application.FirstName,
                LastName = application.LastName,
                Age = application.Age,
                PhoneNumber = application.PhoneNumber,
                Country = application.Country,
                City = application.City,
                IsFirstTime = application.IsFirstTime,
            };
        }
    }
}
