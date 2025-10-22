
using System.ComponentModel.DataAnnotations;

namespace AtharPlatform.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string MessageContent { get; set; }

        public bool IsRead { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        public NotificationSender Sender { get; set; }
        public virtual List<NotificationReceive> ReceiveNotifications { get; set; } = new();
    }
}
