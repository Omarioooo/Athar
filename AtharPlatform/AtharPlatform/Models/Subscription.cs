using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; } = DateTime.UtcNow;

        [Required]
        public double Amount { get; set; }

        [ForeignKey(nameof(Charity))]
        public int charityID { get; set; }
        public Charity Charity { get; set; }


        [ForeignKey(nameof(Donor))]
        public string donornID { get; set; }
        public Donor Donor { get; set; }


        [ForeignKey(nameof(SubscribtionType))]
        public string Type { get; set; }
        public SubscribtionType SubscribtionType { get; set; }


    }
}
