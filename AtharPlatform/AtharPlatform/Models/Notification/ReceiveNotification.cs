using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class ReceiveNotification
    {
        [ForeignKey("Notification")]
        public int NotificationId { get; set; }
        public Notification Notification { get; set; }


        public DateTime ReceivingDate { get; set; }


        [ForeignKey("UserAccount")]
        public int AccountId { get; set; }
        public UserAccount UserAccount { get; set; }
    }
}
