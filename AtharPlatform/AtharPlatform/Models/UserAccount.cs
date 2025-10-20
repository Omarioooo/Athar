using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace AtharPlatform.Models
{
    public class UserAccount : IdentityUser
    {
        
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; }

        public Byte[] ProfileImage { get; set; }


        [ForeignKey("charity")]
        public string AccountId { get; set; }
        public Charity charity { get; set; }

        public List<SendNotification> SendNotifications { get; set; }
        public List<ReceiveNotification> ReceiveNotifications { get; set; }

        public List<UserRoles> UserRoles { get; set; }


    }
}

