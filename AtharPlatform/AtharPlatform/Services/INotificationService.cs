using AtharPlatform.DTO;
using AtharPlatform.Models.Enum;

namespace AtharPlatform.Services
{
    public interface INotificationService
    {
        Task SendNotificationAsync(int senderId, List<int> receiverIds, NotificationsTypeEnum type);
       
        Task<List<NotificationReceiver>> GetUserNotificationsAsync(int userId);
        Task<List<NotificationMessageDto>> GetUserNotificationsSummaryAsync(int userId);
        Task<List<Notification>> GetUserNotificationsFullAsync(int userId);
    }
}