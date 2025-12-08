namespace AtharPlatform.DTOs
{
    public class NotificationFullDto
    {
        public int Id { get; set; }
        public string Message { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public SenderDto? Sender { get; set; }
        public List<ReceiverDto> Receivers { get; set; } = new();
    }
    public class SenderDto
    {
        public int SenderId { get; set; }
        public string? UserName { get; set; }
    }

    public class ReceiverDto
    {
        public int ReceiverId { get; set; }
        public string? UserName { get; set; }
        public bool? IsRead { get; set; }
    }
}
