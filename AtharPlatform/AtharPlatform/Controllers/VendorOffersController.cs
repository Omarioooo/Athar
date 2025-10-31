using AtharPlatform.DTOs;
using AtharPlatform.Models;
using AtharPlatform.Models.Enums;
using AtharPlatform.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AtharPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorOffersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public VendorOffersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get all offers for a specific campaign
        /// </summary>
        [HttpGet("by-campaign/{campaignId}")]
        public async Task<IActionResult> GetByCampaign(int campaignId)
        {
            if (campaignId <= 0)
                return BadRequest(new { Message = "Invalid campaign ID." });

            try
            {
                var offers = await _unitOfWork.VendorOffers.GetByCampaignAsync(campaignId);

                if (offers == null || !offers.Any())
                    return NotFound(new { Message = "No offers found for this campaign." });

                var offersDto = offers.Select(o => MapToDTO(o)).ToList();
                return Ok(offersDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching offers.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get offer by Id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Invalid offer ID." });

            try
            {
                var offer = await _unitOfWork.VendorOffers.GetAsync(id);

                if (offer == null)
                    return NotFound(new { Message = "Offer not found." });

                return Ok(MapToDTO(offer));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching the offer.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new vendor offer
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VendorOfferDTO offerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (offerDto.Quantity < 1)
                return BadRequest(new { Message = "Quantity must be at least 1." });

            if (offerDto.PriceBeforDiscount < 0 || offerDto.PriceAfterDiscount < 0)
                return BadRequest(new { Message = "Price values cannot be negative." });

            try
            {
                var offer = new VendorOffers
                {
                    VendorName = offerDto.VendorName,
                    PhoneNumber = offerDto.PhoneNumber,
                    Country = offerDto.Country,
                    City = offerDto.City,
                    ItemName = offerDto.ItemName,
                    Quantity = offerDto.Quantity,
                    Description = offerDto.Description,
                    PriceBeforDiscount = offerDto.PriceBeforDiscount,
                    PriceAfterDiscount = offerDto.PriceAfterDiscount,
                    CharityVendorOfferId = offerDto.CharityVendorOfferId,
                    Status = OfferStatus.Pending
                };

                await _unitOfWork.VendorOffers.AddAsync(offer);
                await _unitOfWork.SaveAsync();

                return CreatedAtAction(nameof(GetById), new { id = offer.Id }, MapToDTO(offer));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the offer.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Update offer status (Approve / Reject)
        /// </summary>
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] OfferStatus status)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Invalid offer ID." });

            try
            {
                var offer = await _unitOfWork.VendorOffers.GetAsync(id);

                if (offer == null)
                    return NotFound(new { Message = "Offer not found." });

                offer.Status = status;
                await _unitOfWork.VendorOffers.Update(offer);
                await _unitOfWork.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the offer status.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Delete an offer
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Invalid offer ID." });

            try
            {
                var offer = await _unitOfWork.VendorOffers.GetAsync(id);

                if (offer == null)
                    return NotFound(new { Message = "Offer not found." });

                await _unitOfWork.VendorOffers.DeleteAsync(id);
                await _unitOfWork.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the offer.", Error = ex.Message });
            }
        }

        // Manual DTO mapping method to reduce repetition
        private VendorOfferDTO MapToDTO(VendorOffers o) => new VendorOfferDTO
        {
            Id = o.Id,
            VendorName = o.VendorName,
            PhoneNumber = o.PhoneNumber,
            Country = o.Country,
            City = o.City,
            ItemName = o.ItemName,
            Quantity = o.Quantity,
            Description = o.Description,
            PriceBeforDiscount = o.PriceBeforDiscount,
            PriceAfterDiscount = o.PriceAfterDiscount,
            Status = o.Status,
            CharityVendorOfferId = o.CharityVendorOfferId
        };
    }
}
