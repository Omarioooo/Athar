
using System.ComponentModel.DataAnnotations;

namespace AtharPlatform.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Message { get; set; } = null!;

        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual NotificationSender Sender { get; set; } = null!;
        public virtual List<NotificationReceiver> Receivers { get; set; } = new();
    }
}
