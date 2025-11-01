using AtharPlatform.DTO;
using AtharPlatform.DTOs;
using AtharPlatform.Models.Enums;
using AtharPlatform.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace AtharPlatform.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJWTServices _jwtServices;
        private readonly UserManager<UserAccount> _userManager;

        public AccountService(IUnitOfWork unitOfWork, IJWTServices jwtServices,
            UserManager<UserAccount> userManager)
        {
            _unitOfWork = unitOfWork;
            _jwtServices = jwtServices;
            _userManager = userManager;
        }

        public async Task<IdentityResult> CharityRegisterAsync(CharityRegisterDto model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model), "Registration data cannot be null.");

            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
                throw new ArgumentException("Email and Password are required.");

            var userName = new MailAddress(model.Email).User;

            using MemoryStream stream = new MemoryStream();
            if (model.ProfileImage != null)
            {
                await model.ProfileImage.CopyToAsync(stream);
            }

            var user = new UserAccount
            {
                UserName = userName,
                Email = model.Email.ToLowerInvariant(),
                Country = model.Country,
                City = model.City,
                ProfileImage = stream.ToArray(),
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                throw new InvalidOperationException("Failed to create user: " +
                    string.Join(", ", result.Errors.Select(e => e.Description)));


            await _userManager.AddToRoleAsync(user, RolesEnum.Charity.ToString());

            using MemoryStream docStream = new MemoryStream();
            if (model.VerificationDocument != null)
            {
                await model.VerificationDocument.CopyToAsync(docStream);
            }

            var Charity = new Charity
            {
                Id = user.Id,
                Name = model.CharityName,
                Description = model.Description,
                VerificationDocument = docStream.ToArray()
            };

            await _unitOfWork.Charity.AddAsync(Charity);

            return result;
        }

        public async Task<IdentityResult> PersonRegisterAsync(PersonRegisterDto model, RolesEnum role)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model), "Registration data cannot be null.");

            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
                throw new ArgumentException("Email and Password are required.");

            var userName = new MailAddress(model.Email).User;

            using MemoryStream stream = new MemoryStream();
            if (model.ProfileImage != null)
            {
                await model.ProfileImage.CopyToAsync(stream);
            }

            var user = new UserAccount
            {
                UserName = userName,
                Email = model.Email.ToLowerInvariant(),
                Country = model.Country,
                City = model.City,
                ProfileImage = stream.ToArray(),
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                throw new InvalidOperationException("Failed to create user: " +
                    string.Join(", ", result.Errors.Select(e => e.Description)));


            await _userManager.AddToRoleAsync(user, role.ToString());

            var Donor = new Donor
            {
                Id = user.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Country = model.Country,
                City = model.City,
            };

            await _unitOfWork.Donor.AddAsync(Donor);

            return result;
        }

        public async Task<TokenDto> LogInAsync(LoginDto model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model), "login data cannot be null.");

            if (string.IsNullOrWhiteSpace(model.UserNameOrEmail) || string.IsNullOrWhiteSpace(model.Password))
                throw new ArgumentException("Email and Password are required.");

            UserAccount? user;
            string userNameOrEmail = model.UserNameOrEmail.Trim();

            if (new EmailAddressAttribute().IsValid(userNameOrEmail))
                user = await _userManager.FindByEmailAsync(userNameOrEmail.ToLowerInvariant());
            else
                user = await _userManager.FindByNameAsync(userNameOrEmail);

            if (user == null)
                throw new InvalidOperationException("Invalid Username or Email.");

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
                throw new UnauthorizedAccessException("Invalid password.");

            // Create token
            var token = await _jwtServices.CreateJwtTokenAsync(user);

            return token;
        }

        public async Task<UserAccount> FindByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.");

            var account = await _userManager.FindByEmailAsync(email.ToLowerInvariant());

            if (account == null)
                throw new InvalidOperationException("No account found with the specified email.");

            return account;
        }

        public async Task<UserAccount> FindByNameAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("Username cannot be empty.");

            var account = await _userManager.FindByNameAsync(userName);

            if (account == null)
                throw new InvalidOperationException("No account found with the specified username.");

            return account;
        }
    }
}
