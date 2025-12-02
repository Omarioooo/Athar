using AtharPlatform.DTO;
using AtharPlatform.DTOs;
using AtharPlatform.Models.Enums;

namespace AtharPlatform.Services
{
    public interface IDonorService
    {
        Task<DonorProfileDto> GetDonorById(int id);
        Task<bool> DonateToCharityAsync(DonationDto model);
        Task<bool> DonateToCampaignAsync(DonationDto model);

    }
}
