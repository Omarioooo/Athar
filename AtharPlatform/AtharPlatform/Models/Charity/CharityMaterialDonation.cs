using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{

    //Weak Entity
    public class CharityMaterialDonation
    {

        [Key,ForeignKey("MaterialDonation")]
        public int MaterialDonationID { get; set; }
        public MaterialDonation MaterialDonation { get; set; }

        [ForeignKey("charity")]
        public int CharityId { get; set; }
        public Charity charity { get; set; }


        public DateTime Date { get; set; }

    }
}
