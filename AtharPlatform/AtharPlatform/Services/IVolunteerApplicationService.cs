using AtharPlatform.DTOs;

namespace AtharPlatform.Services
{
    public interface IVolunteerApplicationService
    {
        Task<VolunteerApplicationDTO> ApplyAsync(VolunteerApplicationDTO dto);
        Task<VolunteerApplicationDTO?> GetByIdAsync(int id);
    }
}
