using AtharPlatform.DTOs;

namespace AtharPlatform.Services
{
    public interface ICharityService
    {
        Task<CharityProfileDto?> GetCharityByIdAsync(int id);
        Task<Charity?> GetCharityFullProfileAsync(int id);
    }
}
