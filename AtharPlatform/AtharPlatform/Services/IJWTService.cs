using AtharPlatform.DTO;

namespace AtharPlatform.Services
{
    public interface IJWTService
    {
        Task<TokenDto> CreateJwtTokenAsync(UserAccount user);
    }
}