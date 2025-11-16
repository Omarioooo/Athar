using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class NotificationSender
    {
        [Key]
        [ForeignKey(nameof(Notification))]
        public int NotificationId { get; set; }
        public virtual Notification Notification { get; set; } = null!;

        [ForeignKey(nameof(Sender))]
        public int SenderId { get; set; }
        public virtual UserAccount Sender { get; set; } = null!;
    }
}

