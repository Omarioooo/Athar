namespace AtharPlatform.Models
{
    public class UserAccount : IdentityUser<int>
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

        public byte[]? ProfileImage { get; set; }

        public string? Country { get; set; }

        public string? City { get; set; }

        public virtual List<NotificationReceiver> Receivers { get; set; } = null!;
    }
}

