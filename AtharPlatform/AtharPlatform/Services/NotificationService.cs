using AtharPlatform.DTO;
using AtharPlatform.Hub;
using AtharPlatform.Models.Enum;
using AtharPlatform.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace AtharPlatform.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<UserAccount> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IUnitOfWork unitOfWork, UserManager<UserAccount> userManager,
            IHubContext<NotificationHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _hubContext = hubContext;
        }

        public async Task<List<NotificationReceiver>> GetUserNotificationsAsync(string id)
        {
            var notifications = await _unitOfWork.Notifications.GetNotificationsByUserAsync(id);

            var notificationReceivers = new List<NotificationReceiver>();

            foreach (var notification in notifications)
            {
                var receiver = notification.Receivers.FirstOrDefault(r => r.ReceiverId == id);
                if (receiver != null)
                {
                    notificationReceivers.Add(receiver);
                }
            }
            return notificationReceivers;
        }

        public async Task SendNotificationAsync(string senderId, List<string> receiverIds, NotificationsTypeEnum type)
        {
            var sender = await _userManager.FindByIdAsync(senderId.ToString());

            if (sender == null)
                throw new ArgumentException("Sender not found");

            // Get NotificationType from DB
            var notificationType = await _unitOfWork.NotificationsTypes.GetNotificationTypeByIdAsync((int)type);

            if (notificationType == null)
                throw new ArgumentException($"NotificationType {(int)type} not found in database");

            // Generate message content
            var messageDto = await CreateMessage(type, sender.UserName);


            // Create Notification
            var notification = new Notification
            {
                Message = messageDto.Message,
                TypeId = notificationType.Id,
                Date = messageDto.CreatedAt,
                IsRead = false,
                IsDeleted = false,
            };
            await _unitOfWork.Notifications.AddAsync(notification);

            // Add Sender
            var notificationSender = new NotificationSender
            {
                SenderId = senderId,
                Notification = notification
            };
            await _unitOfWork.Notifications.AddSenderAsync(notificationSender);

            // Add Receivers
            var notificationReceivers = receiverIds.Select(rid => new NotificationReceiver
            {
                ReceiverId = rid,
                Notification = notification
            }).ToList();
            await _unitOfWork.Notifications.AddReceiversAsync(notificationReceivers);

            // Save all changes
            await _unitOfWork.SaveAsync();

            // Send notification
            await _hubContext.Clients
                .Users(receiverIds.Select(id => id.ToString()))
                .SendAsync("ReceiveNotification", messageDto);
        }

        private Task<NotificationMessageDto> CreateMessage(NotificationsTypeEnum type, string senderName)
        {
            var time = DateTime.UtcNow;

            NotificationMessageDto message = type switch
            {
                NotificationsTypeEnum.NewCampagin => new NotificationMessageDto
                {
                    Message = $"New Campagin added by {senderName} at {time}. Check it out!",
                    CreatedAt = time
                },
                NotificationsTypeEnum.AdminApproved => new NotificationMessageDto
                {
                    Message = $"Your Charity has been approved by admin {senderName} at {time}.",
                    CreatedAt = time
                },
                NotificationsTypeEnum.AdminRejected => new NotificationMessageDto
                {
                    Message = $"Your Charity has been rejected by admin {senderName} at {time}.",
                    CreatedAt = time
                },
                NotificationsTypeEnum.NewCharity => new NotificationMessageDto
                {
                    Message = $"A new Charity {senderName} need to join at {time}.",
                    CreatedAt = time
                },
                NotificationsTypeEnum.NewSubscriber => new NotificationMessageDto
                {
                    Message = $"A new subscriber: {senderName} at {time}.",
                    CreatedAt = time
                },
                _ => new NotificationMessageDto
                {
                    Message = $"You have a new notification from {senderName} at {time}.",
                    CreatedAt = time
                }
            };

            return Task.FromResult(message);
        }
    }
}
