using AtharPlatform.DTOs;
using AtharPlatform.Models.Enums;

namespace AtharPlatform.Services
{
    public interface IDonorService
    {
        Task<bool> DonateToCharityAsync(DonationDto model);
        Task<bool> DonateToCampaignAsync(DonationDto model);
        Task<List<int>> GetFollowsAsync(int donorId);
        Task<List<int>> GetSubscriptionsAsync(int donorId);



    }
}
