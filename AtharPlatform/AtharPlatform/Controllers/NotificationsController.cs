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

        [HttpGet("user/{userId}/notifications")]
        public async Task<IActionResult> GetUserNotificationsSummary(int userId)
        {
            var notifications = await _notificationService.GetUserNotificationsSummaryAsync(userId);

            if (notifications == null || notifications.Count == 0)
                return NotFound(new { message = "No notifications found." });

            return Ok(notifications);
        }

        [HttpGet("notification/{id}")]
        public async Task<IActionResult> GetNotificationById(int id)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);

            if (notification == null)
                return NotFound();

            return Ok(notification);
        }
    }
}
