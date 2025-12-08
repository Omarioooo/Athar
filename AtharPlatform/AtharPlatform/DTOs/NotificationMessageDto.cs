namespace AtharPlatform.DTO
{
    public class NotificationMessageDto
    {
        public string? Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? SenderName { get; set; }
    }
}
