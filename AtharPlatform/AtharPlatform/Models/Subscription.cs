using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [ForeignKey("Charity")]
        public int charityID { get; set; }
        public Charity Charity { get; set; }



        [ForeignKey("Charity")]
        public string SubscribtionID { get; set; }
        public User user { get; set; }


        [ForeignKey("subscribtionType")]
        public string SubscribtionType { get; set; }
        public SubscribtionType subscribtionType { get; set; }


    }
}
