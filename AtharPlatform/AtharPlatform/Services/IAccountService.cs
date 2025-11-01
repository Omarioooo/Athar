using AtharPlatform.DTO;
using AtharPlatform.DTOs;
using AtharPlatform.Models.Enums;

namespace AtharPlatform.Services
{
    public interface IAccountService
    {
        /// <summary>
        /// Registers a new charity organization asynchronously.
        /// </summary>
        /// <param name="model">The data transfer object containing the charity's registration details</param>
        /// <returns>An <see cref="IdentityResult"/> indicating the success or failure of the registration process.</returns>
        Task<IdentityResult> CharityRegisterAsync(CharityRegisterDto model);

        /// <summary>
        /// Registers a new person with the specified role asynchronously.
        /// </summary>
        /// <param name="model">The data transfer object containing the person's registration details.</param>
        /// <param name="role">The role to assign to the person upon registration.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IdentityResult"/>
        /// indicating the success or failure of the registration process.</returns>
        Task<IdentityResult> PersonRegisterAsync(PersonRegisterDto model, RolesEnum role);

        /// <summary>
        /// Authenticates a user asynchronously and returns an access token.
        /// </summary>
        /// <param name="model">The login details required for authentication, including username and password.</param>
        /// <returns>A <see cref="TokenDto"/> containing the access token and related information if authentication is
        /// successful.</returns>
        Task<TokenDto> LogInAsync(LoginDto model);

        /// <summary>
        /// Asynchronously retrieves a user account by the specified email address.
        /// </summary>
        /// <param name="mail">The email address of the user account to find. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="UserAccount"/>
        /// associated with the specified email, or <see langword="null"/> if no account is found.</returns>
        Task<UserAccount> FindByEmailAsync(string mail);

        /// <summary>
        /// Asynchronously retrieves a user account by the specified username.
        /// </summary>
        /// <param name="userName">The username of the account to find. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="UserAccount"/>
        /// associated with the specified username, or <see langword="null"/> if no such account exists.</returns>
        Task<UserAccount> FindByNameAsync(string userName);
    }
}
