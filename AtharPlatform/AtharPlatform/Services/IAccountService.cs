using AtharPlatform.DTO;
using AtharPlatform.DTOs;
using AtharPlatform.Models.Enums;

namespace AtharPlatform.Services
{
    public interface IAccountService
    {
        Task<IdentityResult> CharityRegisterAsync(CharityRegisterDto model);
        Task<IdentityResult> PersonRegisterAsync(PersonRegisterDto model, RolesEnum role);
        Task<object> LogInAsync(LoginDto model);

        Task<UserAccount> FindByEmailAsync(string mail);

        Task<UserAccount> FindByNameAsync(string userName);
    }
}
