using AtharPlatform.DTO;

namespace AtharPlatform.Hubs
{
    public interface INotificationHub
    {
        
        Task SendMessagetousers(List<int> userIds, NotificationMessageDto message);
        Task BroadCastMessage(NotificationMessageDto message);
    }
}
