using AtharPlatform.Models.Enum;

namespace AtharPlatform.Services
{
    public interface INotificationService
    {
        Task SendNotificationAsync(string senderId, List<string> receiverIds, NotificationsTypeEnum type);
        Task<List<NotificationReceiver>> GetUserNotificationsAsync(string userId);
    }
}