using AtharPlatform.DTOs;

namespace AtharPlatform.Services
{
    public interface IJWTServices
    {
        Task<TokenDto> CreateJwtTokenAsync(UserAccount user);
    }
}