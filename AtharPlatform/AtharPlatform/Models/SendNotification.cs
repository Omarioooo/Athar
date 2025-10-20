using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class SendNotification
    {
        [Key,ForeignKey("Notification")]
        public int NotificationId { get; set; }
        public Notification Notification { get; set; }


        public DateTime SendingDate { get; set; }



        [ForeignKey("UserAccount")]
        public string AccountId { get; set; }
        public UserAccount UserAccount { get; set; }
    }
}
