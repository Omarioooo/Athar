using System.ComponentModel.DataAnnotations;

namespace AtharPlatform.Models
{
    public class UserAccount : IdentityUser
    {
        [Key]
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

        public Byte[]? ProfileImage { get; set; }

        public virtual List<NotificationReceive> ReceiveNotifications { get; set; } = new();
    }
}

