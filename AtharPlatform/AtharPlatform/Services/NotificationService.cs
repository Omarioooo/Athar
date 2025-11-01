using AtharPlatform.DTO;
using AtharPlatform.Hubs;
using AtharPlatform.Models.Enum;
using AtharPlatform.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace AtharPlatform.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<UserAccount> _userManager;
        private readonly INotificationHub _hub;

        public NotificationService(IUnitOfWork unitOfWork,
            UserManager<UserAccount> userManager, INotificationHub hub)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _hub = hub;
        }

        public async Task<List<NotificationReceiver>> GetUserNotificationsAsync(int id)
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

        public async Task SendNotificationAsync(int senderId, List<int> receiverIds, NotificationsTypeEnum type)
        {
            var sender = await _userManager.FindByIdAsync(senderId.ToString());

            if (sender == null)
                throw new ArgumentException("Sender not found");

            // Get NotificationType from DB
            var notificationType = await _unitOfWork.NotificationTypes.GetNotificationTypeByIdAsync((int)type);

            if (notificationType == null)
                throw new ArgumentException($"NotificationType {(int)type} not found in database");

            // Generate message content
            var messageDto = await CreateMessage(type, sender.UserName);


            // Create Notification
            var notification = new Notification
            {
                Message = messageDto.Message,
                TypeId = notificationType.Id,
                CreatedAt = messageDto.CreatedAt,
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
            var notificationReceivers =
                receiverIds.Select(rid => new NotificationReceiver
                {
                    ReceiverId = rid,
                    Notification = notification
                }).ToList();
            await _unitOfWork.Notifications.AddReceiversAsync(notificationReceivers);

            // Save all changes
            await _unitOfWork.SaveAsync();

            // Send notification
            await _hub.SendMessage(receiverIds, messageDto);
        }

        private Task<NotificationMessageDto> CreateMessage(NotificationsTypeEnum type, string senderName)
        {
            var time = DateTime.UtcNow;

            var message = type switch
            {
                NotificationsTypeEnum.NewCharity => new NotificationMessageDto
                {
                    Message = $"A new charity ({senderName}) has signed up at {time}. Please review it.",
                    CreatedAt = time
                },
                NotificationsTypeEnum.AdminApproved => new NotificationMessageDto
                {
                    Message = $"Your charity has been approved by admin {senderName} at {time}.",
                    CreatedAt = time
                },
                NotificationsTypeEnum.AdminRejected => new NotificationMessageDto
                {
                    Message = $"Your charity has been rejected by admin {senderName} at {time}.",
                    CreatedAt = time
                },
                NotificationsTypeEnum.NewCampagin => new NotificationMessageDto
                {
                    Message = $"A new campaign has been launched by {senderName} at {time}. Check it out!",
                    CreatedAt = time
                },
                NotificationsTypeEnum.NewSubscriber => new NotificationMessageDto
                {
                    Message = $"A new subscriber ({senderName}) joined at {time}.",
                    CreatedAt = time
                },
                NotificationsTypeEnum.NewFollower => new NotificationMessageDto
                {
                    Message = $"A new follower ({senderName}) followed you at {time}.",
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
