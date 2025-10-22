using System.ComponentModel.DataAnnotations;

namespace AtharPlatform.Models
{
    public class SubscribtionType
    {
        [Key]
        public int Id { get; set; }

        public string Type { get; set; }

        public virtual List<Subscription> Subscriptions { get; set; } = new();
    }
}
