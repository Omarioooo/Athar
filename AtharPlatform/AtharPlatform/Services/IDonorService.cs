using AtharPlatform.DTO;
using AtharPlatform.Models.Enums;

namespace AtharPlatform.Services
{
    public interface IDonorService
    {
        Task<bool> DonateToCharityAsync(DonationDto model);
        Task<bool> DonateToCampaignAsync(DonationDto model);

    }
}
