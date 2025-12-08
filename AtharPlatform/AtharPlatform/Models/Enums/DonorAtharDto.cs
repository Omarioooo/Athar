using AtharPlatform.DTOs;

namespace AtharPlatform.Models.Enums
{
    public class DonorAtharDto
    {
        public decimal DonationsAmount { get; set; } = 0;
       public List<DonorDonationAtharHistoryDto> donations = new();
       public List<FollowAtharHistoryDto> follows = new();
    }

}
