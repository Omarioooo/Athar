using AtharPlatform.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AtharPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("all/{id}")]
        public async Task<IActionResult> GetUserNotificationsSummary(int id)
        {
            var notifications = await _notificationService.GetUserNotificationsSummaryAsync(id);

            if (notifications == null || notifications.Count == 0)
                return NotFound(new { message = "No notifications found." });

            return Ok(notifications);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserNotificationsFull(int userId)
        {
            var notifications = await _notificationService.GetNotificationByIdAsync(userId);
            return Ok(notifications);
        }
    }
}
