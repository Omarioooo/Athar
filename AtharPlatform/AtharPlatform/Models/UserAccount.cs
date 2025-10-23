using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AtharPlatform.Models
{
    public class UserAccount : IdentityUser
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

        public Byte[]? ProfileImage { get; set; }

        public virtual List<NotificationReceive> ReceiveNotifications { get; set; } = new();
    }
}

