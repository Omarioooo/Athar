using System.Threading.Tasks;
using AtharPlatform.DTOs;
using AtharPlatform.Repositories;
using AtharPlatform.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AtharPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonorController : ControllerBase
    {
        public IDonorService _donorService;

        public DonorController(IDonorService donorService)
        {
            _donorService = donorService;
        }


        [HttpGet("athar/{id}")]
        public async Task<IActionResult> GetDonorAthar(int id)
        {
            try
            {
                var model = await _donorService.GetAtharByDonorIdAsync(id);

                if (model == null)
                    return BadRequest("field");

                /// return Ok(model.follows);
                return Ok(new
                {
                    amount = model.DonationsAmount,
                    follows = model.follows,
                    donations = model.donations
                });
            }
            catch
            {
                return BadRequest("field");
            }
        }


        [HttpGet("donorProfile/{id}")]
        public async Task<IActionResult> GetDonorProfile(int id)
        {
            try
            {
                var profile = await _donorService.GetDonorByIdAsync(id);

                return Ok(profile);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("profile-image/{donorId}")]
        public async Task<IActionResult> GetProfileImage(int donorId)
        {
            var donor = await _donorService.GetDonorFullProfileAsync(donorId);

            if (donor?.Account?.ProfileImage == null || donor.Account.ProfileImage.Length == 0)
                return Ok("No Photo");

            return File(donor.Account.ProfileImage, "image/PNG");
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateDonor(int id, [FromForm] DonorUpdateDto dto)
        {
            try
            {
                var result = await _donorService.UpdateDonorAsync(id, dto);
                if (!result)
                    return BadRequest("Update failed");

                return Ok("Donor updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteDonor(int id)
        {
            try
            {
                var result = await _donorService.DeleteDonorAsync(id);

                if (!result)
                    return NotFound(new { message = "Donor not found." });

                return Ok(new { message = "Donor deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("info/{id}")]
         public async Task<IActionResult> getDonorInfo(int id)
        {
            try
            {
                var result = await _donorService.GetDonorInfoByIdAsync(id);

                if (result == null)
                    return NotFound(new { message = "Donor not found." });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
