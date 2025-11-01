using AtharPlatform.DTOs;

namespace AtharPlatform.Services
{
    public interface IDonationService
    {
        Task<string> DonateToCharityAsync(DonationDto model);
        Task<string> DonateToCampaignAsync(DonationDto model);
    }
}