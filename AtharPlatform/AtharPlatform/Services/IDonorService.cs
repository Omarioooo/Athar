using AtharPlatform.DTO;
using AtharPlatform.DTOs;
using AtharPlatform.Models.Enums;

namespace AtharPlatform.Services
{
    public interface IDonorService
    {
        Task<DonorProfileDto> GetDonorByIdAsync(int id);
        Task<DonorInfoDto> GetDonorInfoByIdAsync(int id);
        Task<DonorAtharDto> GetAtharByDonorIdAsync(int id);
        Task<Donor> GetDonorFullProfileAsync(int id);
        Task<bool> DonateToCharityAsync(DonationDto model);
        Task<bool> DonateToCampaignAsync(DonationDto model);
        Task<bool> UpdateDonorAsync(int donorId, DonorUpdateDto dto);
        Task<bool> DeleteDonorAsync(int id);
    }
}
