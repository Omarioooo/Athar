using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class NotificationReceive
    {
        [Key]
        [ForeignKey(nameof(Notification))]
        public int NotificationId { get; set; }
        public virtual Notification Notification { get; set; } = new();


        [ForeignKey(nameof(Receiver))]
        public string ReceiverId { get; set; }
        public virtual UserAccount Receiver { get; set; } = new();
    }
}
