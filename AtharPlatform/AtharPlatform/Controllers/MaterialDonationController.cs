using AtharPlatform.DTOs;
using AtharPlatform.Models;
using AtharPlatform.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AtharPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MaterialDonationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public MaterialDonationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get material donation by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Invalid donation ID." });

            try
            {
                var donation = await _unitOfWork.MaterialDonations.GetAsync(id);
                if (donation == null)
                    return NotFound(new { Message = "Material donation not found." });

                return Ok(MapToDTO(donation));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while fetching the donation.",
                    Error = ex.Message
                });
            }
        }

        /// <summary>
        /// Submit a new material donation offer
        /// </summary>
        [HttpPost("offer")]
        public async Task<IActionResult> Offer([FromBody] MaterialDonationDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var charity = await _unitOfWork.CharityMaterialDonations
                .GetByIdAsync(dto.MaterialDonationId);

            if (charity == null)
                return NotFound(new { Message = "Material donation charity not found." });

            var existing = await _unitOfWork.MaterialDonations.GetAllAsync();

            try
            {
                var donation = new MaterialDonation
                {
                    DonorFirstName = dto.DonorFirstName,
                    DonorLastName = dto.DonorLastName,
                    PhoneNumber = dto.PhoneNumber,
                    ItemName = dto.ItemName,
                    Quantity = dto.Quantity,
                    Description = dto.Description,
                    Country = dto.Country,
                    City = dto.City,
                    MeasurementUnit = dto.MeasurementUnit,
                    MaterialDonationId = dto.MaterialDonationId
                };

                await _unitOfWork.MaterialDonations.AddAsync(donation);
                await _unitOfWork.SaveAsync();

                return CreatedAtAction(nameof(GetById), new { id = donation.Id }, MapToDTO(donation));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "خطأ أثناء حفظ عرض التبرع",
                    Error = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Delete a material donation offer
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Invalid donation ID." });

            try
            {
                var donation = await _unitOfWork.MaterialDonations.GetAsync(id);
                if (donation == null)
                    return NotFound(new { Message = "Donation offer not found." });

                await _unitOfWork.MaterialDonations.DeleteAsync(id);
                await _unitOfWork.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while deleting the donation offer.",
                    Error = ex.Message
                });
            }
        }




        private MaterialDonationDTO MapToDTO(MaterialDonation donation)
        {
            return new MaterialDonationDTO
            {
                Id = donation.Id,
                DonorFirstName = donation.DonorFirstName,
                DonorLastName = donation.DonorLastName,
                PhoneNumber = donation.PhoneNumber,
                ItemName = donation.ItemName,
                Quantity = donation.Quantity,
                Description = donation.Description,
                Country = donation.Country,
                City = donation.City,
                MeasurementUnit = donation.MeasurementUnit,
                MaterialDonationId = donation.MaterialDonationId
            };
        }
    }
}