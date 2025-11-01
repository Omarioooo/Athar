using AtharPlatform.DTO;

namespace AtharPlatform.Hubs
{
    public interface INotificationHub
    {
<<<<<<< HEAD
        
        Task SendMessagetousers(List<int> userIds, NotificationMessageDto message);
=======
        Task SendMessageToOneUser(int userId, NotificationMessageDto message);
        Task SendMessageToListOfUseres(List<int> userIds, NotificationMessageDto message);
>>>>>>> master
        Task BroadCastMessage(NotificationMessageDto message);
    }
}
