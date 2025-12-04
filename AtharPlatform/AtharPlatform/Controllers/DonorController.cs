using AtharPlatform.Repositories;
using AtharPlatform.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
    }
}
