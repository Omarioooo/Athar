using AtharPlatform.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AtharPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowController : ControllerBase
    {
        private readonly IFollowService _followService;
        private readonly IAccountContextService _accountContextService;

        public FollowController(IFollowService followService, IAccountContextService accountContextService)
        {
            _followService = followService;
            _accountContextService = accountContextService;
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Follow([FromRoute] int id)
        {
            try
            {
                // get the current user
                var userId = _accountContextService.GetCurrentAccountId();

                var isSuccess = await _followService.FollowAsync(userId, id);

                if (!isSuccess)
                    return BadRequest(new { message = "Cannot follow this charity." });

                return Ok(new { message = "Following successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                // Charity not found
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // Already following this charity
                return Conflict(new { message = ex.Message });
            }
            catch (Exception)
            {
                // Unexpected error
                return StatusCode(500, new { message = "An unexpected error occurred while trying to follow the charity." });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Unfollow(int id)
        {
            try
            {
                // get the current user
                var userId = _accountContextService.GetCurrentAccountId();

                var isSuccess = await _followService.UnFollowAsync(userId, id);

                if (!isSuccess)
                    return BadRequest(new { message = "Cannot unfollow this charity." });

                return Ok(new { message = "Unfollowed successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                // Charity not found
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // Not following this charity
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                // Unexpected error
                return StatusCode(500, new { message = "An unexpected error occurred while trying to unfollow the charity." });
            }
        }
    }
}
