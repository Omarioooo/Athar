using AtharPlatform.DTO;
using Microsoft.AspNetCore.SignalR;

namespace AtharPlatform.Hubs
{
    public class NotificationHubHelper : Hub
    {
        public async Task SendMessage(NotificationMessageDto message)
        {
            await Clients.Others.SendAsync("ReceiveNotification", message);
        }
    }
}
