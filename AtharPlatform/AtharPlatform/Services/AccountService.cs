using AtharPlatform.DTO;
using AtharPlatform.Models.Enums;
using AtharPlatform.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;


namespace AtharPlatform.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJWTService _jwtServices;
        private readonly UserManager<UserAccount> _userManager;

        public AccountService(IUnitOfWork unitOfWork, IJWTService jwtServices,
            UserManager<UserAccount> userManager)
        {
            _unitOfWork = unitOfWork;
            _jwtServices = jwtServices;
            _userManager = userManager;
        }

        public async Task<IdentityResult> CharityRegisterAsync(CharityRegisterDto model)
        {
            // Check the model
            if (model == null)
                throw new ArgumentNullException(nameof(model), "Registration data cannot be null.");

            // check user data
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
                throw new ArgumentException("Email and Password are required.");

            // Dealing with files (ProfileImage)
            using MemoryStream stream = new MemoryStream();
            if (model.ProfileImage != null)
            {
                await model.ProfileImage.CopyToAsync(stream);
            }

            // The account entity
            var account = new UserAccount
            {
                UserName = new MailAddress(model.Email).User,
                Email = model.Email.ToLowerInvariant(),
                Country = model.Country,
                City = model.City,
                ProfileImage = stream.ToArray(),
                CreatedAt = DateTime.UtcNow
            };

            // Create the account using the identity
            var result = await _userManager.CreateAsync(account, model.Password);

            if (!result.Succeeded)
                throw new InvalidOperationException("Failed to create user: " +
                    string.Join(", ", result.Errors.Select(e => e.Description)));

            // Assign roles to the account (The account is for charity)
            await _userManager.AddToRoleAsync(account, RolesEnum.Charity.ToString());

            // Dealing with files (verification doc)
            using MemoryStream docStream = new MemoryStream();
            if (model.VerificationDocument != null)
            {
                await model.VerificationDocument.CopyToAsync(docStream);
            }

            var Charity = new Charity
            {
                Id = account.Id,
                Name = model.CharityName,
                Description = model.Description,
                VerificationDocument = docStream.ToArray()
            };
            await _unitOfWork.Charities.AddAsync(Charity);

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

            var account = new UserAccount
            {
                UserName = userName,
                Email = model.Email.ToLowerInvariant(),
                Country = model.Country,
                City = model.City,
                ProfileImage = stream.ToArray(),
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(account, model.Password);

            if (!result.Succeeded)
                throw new InvalidOperationException("Failed to create user: " +
                    string.Join(", ", result.Errors.Select(e => e.Description)));

            // Explicitly give the Account role as admin or donor
            await _userManager.AddToRoleAsync(account, role.ToString());

            var Donor = new Donor
            {
                Id = account.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Country = model.Country,
                City = model.City,
            };

            await _unitOfWork.Donors.AddAsync(Donor);

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
