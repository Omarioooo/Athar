
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

        public DateTime Time { get; set; }

        public List<SendNotification> SendNotifications { get; set; }
        public List<ReceiveNotification> ReceiveNotifications { get; set; }
    }
}
