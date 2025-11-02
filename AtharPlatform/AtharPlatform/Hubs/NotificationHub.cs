using AtharPlatform.DTO;
using AtharPlatform.Hubs;
using Microsoft.AspNetCore.SignalR;

public class NotificationHub : INotificationHub
{
    private readonly IHubContext<NotificationHubHelper> _hubContext;

    public NotificationHub(IHubContext<NotificationHubHelper> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendMessage(List<int> userIds, NotificationMessageDto message)
    {
        var tasks = userIds.Select(userId =>
            _hubContext.Clients.User(userId.ToString())
                .SendAsync("ReceiveNotification", message)
        );

        await Task.WhenAll(tasks);
    }

    public async Task BroadCastMessage(NotificationMessageDto message)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveNotification", message);
    }
}
