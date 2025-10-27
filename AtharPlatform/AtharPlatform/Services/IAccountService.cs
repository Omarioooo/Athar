using AtharPlatform.DTO;
using AtharPlatform.DTOs;

namespace AtharPlatform.Services
{
    public interface IAccountService
    {
        Task<IdentityResult> CharityRegisterAsync(CharityRegisterDto model);
        Task<IdentityResult> DonorRegisterAsync(DonorRegisterDto model);
        Task<object> LogInAsync(LoginDto model);

        Task<UserAccount> FindByEmailAsync(string mail);

        Task<UserAccount> FindByNameAsync(string userName);
    }
}
