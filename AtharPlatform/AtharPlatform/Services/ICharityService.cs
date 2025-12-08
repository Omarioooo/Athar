using AtharPlatform.DTOs;

namespace AtharPlatform.Services
{
    public interface ICharityService
    {
        Task<bool> UpdateAsync(int id, UpdateCharityDto model);
        Task<CharityProfileDto?> GetCharityByIdAsync(int id);
        Task<Charity?> GetCharityFullProfileAsync(int id);

        Task<CharityViewDto?> GetCharityViewAsync(int id);

        Task<List<CharityApplicationResponseDto>> GetAllApplicationsForCharityAsync(int charityId);
        Task<VendorOfferDTO> GetVendorOfferForCharityByIdAsync(int offerId);
        Task<VolunteerApplicationDTO> GetVolunteerOfferForCharityByIdAsync(int offerId);
    }
}
