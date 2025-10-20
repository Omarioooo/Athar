using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class CampaignDonation
    {
        
        [Key,ForeignKey("Donation")]
        public int DonationId { get; set; }
        public Donation Donation { get; set; }


        public DateTime  Date { get; set; }



        [ForeignKey("Campaign")]
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; }


        [ForeignKey("user")]
        public String DonorId { get; set; }//By default Id in Identity String
        public User user { get; set; }


    }
}
