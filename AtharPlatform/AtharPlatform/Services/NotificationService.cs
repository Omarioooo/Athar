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


            // Generate message content
            var messageDto = await CreateMessage(type, sender.UserName);


            // Create Notification
            var notification = new Notification
            {
                Message = messageDto.Message,
                CreatedAt = messageDto.CreatedAt,
                IsDeleted = false,
            };
            await _unitOfWork.Notifications.AddAsync(notification);
            await _unitOfWork.SaveAsync();

            // Add Sender
            var notificationSender = new NotificationSender
            {
                NotificationId = notification.Id,
                SenderId = senderId
            };
            await _unitOfWork.Notifications.AddSenderAsync(notificationSender);

            // Add Receivers
            var notificationReceivers =
                receiverIds.Select(rid => new NotificationReceiver
                {

                    NotificationId = notification.Id,
                    ReceiverId = rid,
                    IsRead = false
                }).ToList();
            await _unitOfWork.Notifications.AddReceiversAsync(notificationReceivers);

            // Save all changes
            await _unitOfWork.SaveAsync();

            // Send notification
            if (_hub == null)
                throw new Exception("Hub is null");

            await _hub.SendMessage(receiverIds, messageDto);
        }

        private Task<NotificationMessageDto> CreateMessage(NotificationsTypeEnum type, string senderName)
        {
            var time = DateTime.UtcNow;

            //var message = type switch
            //{
            //    NotificationsTypeEnum.NewCharity => new NotificationMessageDto
            //    {
            //        Message = $"A new charity ({senderName}) has signed up at {time}. Please review it.",
            //        CreatedAt = time
            //    },
            //    NotificationsTypeEnum.AdminApproved => new NotificationMessageDto
            //    {
            //        Message = $"Your charity has been approved by admin {senderName} at {time}.",
            //        CreatedAt = time
            //    },
            //    NotificationsTypeEnum.AdminRejected => new NotificationMessageDto
            //    {
            //        Message = $"Your charity has been rejected by admin {senderName} at {time}.",
            //        CreatedAt = time
            //    },
            //    NotificationsTypeEnum.NewCampagin => new NotificationMessageDto
            //    {
            //        Message = $"A new campaign has been launched by {senderName} at {time}. Check it out!",
            //        CreatedAt = time
            //    },
            //    NotificationsTypeEnum.NewSubscriber => new NotificationMessageDto
            //    {
            //        Message = $"A new subscriber ({senderName}) joined at {time}.",
            //        CreatedAt = time
            //    },
            //    NotificationsTypeEnum.NewFollower => new NotificationMessageDto
            //    {
            //        Message = $"A new follower ({senderName}) followed you at {time}.",
            //        CreatedAt = time
            //    },
            //    _ => new NotificationMessageDto
            //    {
            //        Message = $"You have a new notification from {senderName} at {time}.",
            //        CreatedAt = time
            //    }
            //};
            var message = type switch
            {
                NotificationsTypeEnum.NewCharity => new NotificationMessageDto
                {
                    Message = $"تم تسجيل جمعية جديدة ({senderName}) بتاريخ {time}. برجاء مراجعتها.",
                    CreatedAt = time
                },

                NotificationsTypeEnum.AdminApproved => new NotificationMessageDto
                {
                    Message = $"تمت الموافقة على جمعيتك من قبل المسؤول ({senderName}) بتاريخ {time}.",
                    CreatedAt = time
                },

                NotificationsTypeEnum.AdminRejected => new NotificationMessageDto
                {
                    Message = $"تم رفض جمعيتك من قبل المسؤول ({senderName}) بتاريخ {time}.",
                    CreatedAt = time
                },

                NotificationsTypeEnum.NewCampagin => new NotificationMessageDto
                {
                    Message = $"قام ({senderName}) بإطلاق حملة جديدة بتاريخ {time}. يمكنك الاطّلاع عليها الآن!",
                    CreatedAt = time
                },

                NotificationsTypeEnum.NewSubscriber => new NotificationMessageDto
                {
                    Message = $"مشترك جديد ({senderName}) انضم بتاريخ {time}.",
                    CreatedAt = time
                },

                NotificationsTypeEnum.NewFollower => new NotificationMessageDto
                {
                    Message = $"متابع جديد ({senderName}) قام بمتابعتك بتاريخ {time}.",
                    CreatedAt = time
                },

                _ => new NotificationMessageDto
                {
                    Message = $"لديك إشعار جديد من ({senderName}) بتاريخ {time}.",
                    CreatedAt = time
                }
            };


            return Task.FromResult(message);
        }
    }
}
