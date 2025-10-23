using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace AtharPlatform.Models
{
    public class Charity : UserAccount
    {
        [Required]
        public String Name { get; set; }

        public String Description { get; set; }

        [Required]
        public byte[] VerificationDocuments { get; set; }
        public CharityStatusEnum Status { get; set; } = CharityStatusEnum.Pending;
        public virtual List<CharityVolunteer> charityVolunteers { get; set; } = new();
        public virtual List<CharityVendorOffer> charityVendorOffers { get; set; } = new();
        public virtual List<CharityMaterialDonation> charityMaterialDonations { get; set; } = new();
        public virtual List<Subscription> subscription { get; set; } = new();
        public virtual List<CharityCampaign> campaigns { get; set; } = new();

    }
}
