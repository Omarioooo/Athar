using AtharPlatform.DTOs;
using AtharPlatform.Models;
using AtharPlatform.Models.Enums;
using AtharPlatform.Repositories;
using Humanizer;
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

        //[HttpGet("by-Charity/{CharityId}")]
        ////[Authorize(Roles = "CharityAdmin,SuperAdmin")]
        //public async Task<IActionResult> GetByCharity(int CharityId)
        //{
        //    if (CharityId <= 0)
        //        return BadRequest(new { Message = "Invalid Charity ID." });

        //    var offers = await _unitOfWork.CharityVendorOffers.GetByCharityIdAsync(CharityId);

        //    if (offers == null || !offers.Any())
        //        return NotFound(new { Message = "No offers found for this Charity." });

        //    ////  var offersDto = offers.Select(MapToDTO).ToList();
        //    //  return Ok(offers);

        //    var dtoList = offers.Select(o => new
        //    {
        //        o.CharityId,

        //        VendorOffers = o.VendorOffers.Select(va => new
        //        {
        //            va.Id,
        //            va.VendorName,
        //            va.ItemName,
        //            va.Description,
        //            va.PhoneNumber,
        //            va.PriceAfterDiscount,
        //            va.PriceBeforDiscount,
        //            va.City,
        //            va.Country


        //        })

        //    });

        //    return Ok(dtoList);
        //}

        [HttpGet("{id}")]
        //Id for one User
        //// [Authorize(Roles = "CharityAdmin,SuperAdmin")]

        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Invalid offer ID." });

            var offer = await _unitOfWork.VendorOffers.GetAsync(id);

            if (offer == null)
                return NotFound(new { Message = "Offer not found." });

            return Ok(MapToDTO(offer));
        }

        [HttpPost("apply")]
        //[Authorize(Roles = "Donor")]
        public async Task<IActionResult> Create([FromBody] VendorOfferDTO offerDto)
        {

            var charity = await _unitOfWork.Charities.GetByIdAsync(offerDto.CharityId);
            if (charity == null)
                return NotFound("Charity not found");


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (offerDto.Quantity < 1)
                return BadRequest(new { Message = "Quantity must be at least 1." });

            if (offerDto.PriceBeforDiscount < 0 || offerDto.PriceAfterDiscount < 0)
                return BadRequest(new { Message = "Price values cannot be negative." });

            // Check if there's a Vendor Slot
            var slot = await _unitOfWork.CharityVendorOffers.GetSlotByCharityIdAsync(offerDto.CharityId);
            if (slot == null)
            {
                slot = new CharityVendorOffer
                {
                    CharityId = offerDto.CharityId,
                    Date = DateTime.UtcNow
                };
                await _unitOfWork.CharityVendorOffers.AddAsync(slot);
                await _unitOfWork.SaveAsync();
            }


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
                CharityVendorOfferId = slot.CharityId,
                Status = OfferStatus.Pending
            };

            await _unitOfWork.VendorOffers.AddAsync(offer);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetById), new { id = offer.Id }, MapToDTO(offer));
        }

      //  [HttpPut("{id}/status")]
        
        //public async Task<IActionResult> UpdateStatus(int id, [FromBody] OfferStatus status)
        //{
        //    if (id <= 0)
        //        return BadRequest(new { Message = "Invalid offer ID." });

        //    var offer = await _unitOfWork.VendorOffers.GetAsync(id);
        //    if (offer == null)
        //        return NotFound(new { Message = "Offer not found." });

        //    offer.Status = status;
        //    await _unitOfWork.VendorOffers.UpdateAsync(offer);
        //    await _unitOfWork.SaveAsync();

        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        ////[Authorize(Roles = "CharityAdmin")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    if (id <= 0)
        //        return BadRequest(new { Message = "Invalid offer ID." });

        //    var offer = await _unitOfWork.VendorOffers.GetAsync(id);
        //    if (offer == null)
        //        return NotFound(new { Message = "Offer not found." });

        //    await _unitOfWork.VendorOffers.DeleteAsync(id);
        //    await _unitOfWork.SaveAsync();

        //    return NoContent();
        //}

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
        };
    }
}
