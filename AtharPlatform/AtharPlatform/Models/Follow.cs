using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class Follow
    {
        [Key]
        public int Id { get; set; }


        [ForeignKey(nameof(Donor))]
        public int donornID { get; set; }
        public Donor Donor { get; set; }


        [ForeignKey(nameof(Charity))]
        public int charityID { get; set; }
        public Charity Charity { get; set; }

        public DateTime StartDate { get; set; } = DateTime.UtcNow;

    }
}
