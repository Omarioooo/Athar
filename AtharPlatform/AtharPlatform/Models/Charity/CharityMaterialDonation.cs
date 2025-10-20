using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{

    //Weak Entity
    public class CharityMaterialDonation
    {

        [ForeignKey("MaterialDonation")]
        public int MaterialDonationID { get; set; }
        public MaterialDonation MaterialDonation { get; set; }

        [ForeignKey("charity")]
        public int MatrialDonationId { get; set; }
        public Charity charity { get; set; }


        public DateTime Date { get; set; }

    }
}
