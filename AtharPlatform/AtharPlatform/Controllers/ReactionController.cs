using AtharPlatform.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AtharPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReactionController : ControllerBase
    {
        private readonly IReactionService _reactionService;
        private readonly IAccountContextService _acountContextService;

        public ReactionController(IReactionService reactionService, IAccountContextService accountContextService)
        {
            _reactionService = reactionService;
            _acountContextService = accountContextService;
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> React(int id)
        {
            try
            {
                // get the current user
                var userId = _acountContextService.GetCurrentAccountId();

                // react on content
                var isSuccess = await _reactionService.React(userId, id);
                if (!isSuccess)
                    return BadRequest(new { message = "Cannot react to this content." });

                return Ok(new { message = "reacted successfully." });

            }
            catch (KeyNotFoundException ex)
            {
                // content not found
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // Already reacted this content
                return Conflict(new { message = ex.Message });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception)
            {
                // Unexpected error
                return StatusCode(500, new { message = "An unexpected error occurred while trying to react on the content." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveReact(int id)
        {
            try
            {
                // get the current user
                var userId = _acountContextService.GetCurrentAccountId();

                // react on content
                var isSuccess = await _reactionService.RemoveReact(userId, id);
                if (!isSuccess)
                    return BadRequest(new { message = "Cannot remove reaction on this content." });

                return Ok(new { message = "reaction removed successfully." });

            }
            catch (KeyNotFoundException ex)
            {
                // content not found
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // Already reacted this content
                return Conflict(new { message = ex.Message });
            }
            catch (Exception)
            {
                // Unexpected error
                return StatusCode(500, new { message = "An unexpected error occurred while trying to remove reaction on the content." });
            }
        }
    }
}