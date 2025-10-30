using AtharPlatform.DTO;

namespace AtharPlatform.Hubs
{
    public interface INotificationHub
    {
        Task SendMessageToOneUser(int userId, NotificationMessageDto message);
        Task SendMessageToListOfUseres(List<int> userIds, NotificationMessageDto message);
        Task BroadCastMessage(NotificationMessageDto message);
    }
}
