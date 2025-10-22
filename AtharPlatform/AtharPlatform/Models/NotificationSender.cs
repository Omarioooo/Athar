using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class NotificationSender
    {
        [Key]
        [ForeignKey(nameof(Notification))]
        public int NotificationId { get; set; }
        public Notification Notification { get; set; }

        [ForeignKey(nameof(Sender))]
        public string SenderId { get; set; }
        public UserAccount Sender { get; set; }
    }
}

