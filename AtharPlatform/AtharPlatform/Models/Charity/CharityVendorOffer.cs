using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    //Weak Entity
    public class CharityVendorOffer
    {
        [ForeignKey("VendorForm")]
        public int offerID { get; set; }
        public VendorForm VendorForm { get; set; }

        public DateTime Date { get; set; }

        [ForeignKey("Charity")]
        public int CharityID { get; set; }
        public Charity Charity { get; set; }
    }
}
