using AtharPlatform.DTO;

namespace AtharPlatform.Services
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterAsync(RegisterDto model);
        Task<object> LogInAsync(LoginDto model);
    }
}
