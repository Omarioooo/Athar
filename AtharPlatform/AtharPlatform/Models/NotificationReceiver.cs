using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    // Composite PK
    public class NotificationReceiver
    {
        [ForeignKey(nameof(Notification))]
        public int NotificationId { get; set; }
        public virtual Notification Notification { get; set; } = new();


        [ForeignKey(nameof(Receiver))]
        public int ReceiverId { get; set; }
        public virtual UserAccount Receiver { get; set; } = new();
    }
}
