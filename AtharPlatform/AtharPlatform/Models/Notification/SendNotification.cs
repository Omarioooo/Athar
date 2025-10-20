using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class SendNotification
    {
        [ForeignKey("Notification")]
        public int NotificationId { get; set; }
        public Notification Notification { get; set; }


        public DateTime SendingDate { get; set; }



        [ForeignKey("UserAccount")]
        public int AccountId { get; set; }
        public UserAccount UserAccount { get; set; }
    }
}
