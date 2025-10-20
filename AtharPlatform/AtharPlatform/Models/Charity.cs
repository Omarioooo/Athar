using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class Charity : UserAccount
    {
        public String CharityName { get; set; }

        public bool Status { get; set; }



       
        public List<CharityVerificationDocument> VerificationDocuments { get; set; } = new List<CharityVerificationDocument>();



        [Key,ForeignKey("UserAccount")]
        public string AccountId { get; set; }
    
        public UserAccount UserAccount { get; set; }


        public List<CharityVolunteer> charityVolunteers { get; set; }
        public List<CharityVendorOffer> charityVendorOffers { get; set; }
        public List<CharityMaterialDonation> charityMaterialDonations { get; set; }
         public List<Subscription> subscription { get; set; }
        public List<CharityCampaign> campaigns { get; set; }
        public List<UserAccount> userAccount { get; set; }

    }


    public class CharityVerificationDocument
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string DocumentName { get; set; }  // "License", "Certificate"

        public string DocumentPath { get; set; }   // Path or URL to the document
   
    }
}
