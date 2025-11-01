using AtharPlatform.DTO;
using Microsoft.AspNetCore.SignalR;

namespace AtharPlatform.Hubs
{
    public class NotificationHub : Hub, INotificationHub
    {

        public async Task SendMessageToOneUser(int userId, NotificationMessageDto message)
        {
            await Clients.User(userId.ToString()).SendAsync("ReceiveNotification", message);
        }

        public async Task SendMessageToListOfUseres(List<int> userIds, NotificationMessageDto message)

        {
            var tasks = userIds.Select(userId =>
                Clients.User(userId.ToString()).SendAsync("ReceiveNotification", message)
            );

            await Task.WhenAll(tasks);
        }

        public async Task BroadCastMessage(NotificationMessageDto message)
        {
            await Clients.Others.SendAsync("ReceiveNotification", message);
        }
    }
}
