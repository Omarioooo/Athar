using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class Charity : UserAccount
    {

        [Key]
        [ForeignKey(nameof(CharityAccount))]
        public string Id { get; set; }

        [Required]
        public String Name { get; set; }

        public String Description { get; set; }

        [Required]
        public byte[] VerificationDocuments { get; set; }
        public CharityStatusEnum Status { get; set; } = CharityStatusEnum.Pending;

        public UserAccount CharityAccount { get; set; }
        public List<CharityVolunteer> charityVolunteers { get; set; }
        public List<CharityVendorOffer> charityVendorOffers { get; set; }
        public List<CharityMaterialDonation> charityMaterialDonations { get; set; }
        public List<Subscription> subscription { get; set; }
        public List<CharityCampaign> campaigns { get; set; }

    }
}
