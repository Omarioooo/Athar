
using System.ComponentModel.DataAnnotations;

namespace AtharPlatform.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Message { get; set; }

        public bool IsRead { get; set; }

        public bool IsDeleted { get; set; }

        public int TypeId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public NotificationSender Sender { get; set; }
        public virtual List<NotificationReceiver> Receivers { get; set; } = new();
    }
}
