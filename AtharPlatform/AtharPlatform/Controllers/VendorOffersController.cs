using AtharPlatform.DTOs;
using AtharPlatform.Models;
using AtharPlatform.Models.Enums;
using AtharPlatform.Repositories;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("by-campaign/{charityVendorOfferId}")]
        //[Authorize(Roles = "CharityAdmin,SuperAdmin")]
        public async Task<IActionResult> GetByCampaign(int charityVendorOfferId)
        {
            if (charityVendorOfferId <= 0)
                return BadRequest(new { Message = "Invalid campaign ID." });

            var offers = await _unitOfWork.VendorOffers.GetByCampaignAsync(charityVendorOfferId);

            if (offers == null || !offers.Any())
                return NotFound(new { Message = "No offers found for this campaign." });

            var offersDto = offers.Select(MapToDTO).ToList();
            return Ok(offersDto);
        }

        [HttpGet("{id}")]
       // [Authorize(Roles = "CharityAdmin,SuperAdmin")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Invalid offer ID." });

            var offer = await _unitOfWork.VendorOffers.GetAsync(id);

            if (offer == null)
                return NotFound(new { Message = "Offer not found." });

            return Ok(MapToDTO(offer));
        }

        [HttpPost]
        //[Authorize(Roles = "Donor")]
        public async Task<IActionResult> Create([FromBody] VendorOfferDTO offerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (offerDto.Quantity < 1)
                return BadRequest(new { Message = "Quantity must be at least 1." });

            if (offerDto.PriceBeforDiscount < 0 || offerDto.PriceAfterDiscount < 0)
                return BadRequest(new { Message = "Price values cannot be negative." });

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

        [HttpPut("{id}/status")]
        //[Authorize(Roles = "CharityAdmin")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] OfferStatus status)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Invalid offer ID." });

            var offer = await _unitOfWork.VendorOffers.GetAsync(id);
            if (offer == null)
                return NotFound(new { Message = "Offer not found." });

            offer.Status = status;
            await _unitOfWork.VendorOffers.UpdateAsync(offer);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "CharityAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Invalid offer ID." });

            var offer = await _unitOfWork.VendorOffers.GetAsync(id);
            if (offer == null)
                return NotFound(new { Message = "Offer not found." });

            await _unitOfWork.VendorOffers.DeleteAsync(id);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

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
