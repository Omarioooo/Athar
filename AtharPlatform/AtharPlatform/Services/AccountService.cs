using AtharPlatform.DTO;
using AtharPlatform.DTOs;
using AtharPlatform.Models.Enum;
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
        private readonly INotificationService _notificationService;
        private readonly IFileService _fileService;

        public AccountService(IUnitOfWork unitOfWork, IJWTService jwtServices,
            UserManager<UserAccount> userManager, INotificationService notificationService, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _jwtServices = jwtServices;
            _userManager = userManager;
            _notificationService = notificationService;
            _fileService = fileService;
        }

        public async Task<IdentityResult> CharityRegisterAsync(CharityRegisterDto model)
        {
            // Check the model
            if (model == null)
                throw new ArgumentNullException(nameof(model), "Registration data cannot be null.");

            // check user data
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
                throw new ArgumentException("Email and Password are required.");

            // Save profile image as file
            string? profileImageUrl = null;
            if (model.ProfileImage != null)
            {
                profileImageUrl = await _fileService.SaveFileAsync(model.ProfileImage, "charities");
            }

            // The account entity
            var account = new UserAccount
            {
                UserName = new MailAddress(model.Email).User,
                Email = model.Email.ToLowerInvariant(),
                Country = model.Country,
                City = model.City,
                ProfileImage = null, // No longer storing in UserAccount
                CreatedAt = DateTime.UtcNow
            };

            // Create the account using the identity
            var result = await _userManager.CreateAsync(account, model.Password);

            if (!result.Succeeded)
                throw new InvalidOperationException("Failed to create user: " +
                    string.Join(", ", result.Errors.Select(e => e.Description)));

            // Assign the correct platform role for charities
            await _userManager.AddToRoleAsync(account, "CharityAdmin");

            // Dealing with files (verification doc) - keep as byte array for now
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
                VerificationDocument = docStream.ToArray(),
                ImageUrl = profileImageUrl
            };
            await _unitOfWork.Charities.AddAsync(Charity);

            // Send notifcation to admins
            var admins = await _unitOfWork.Donors.GetAllAdminsIdsAsync();
            await _notificationService.SendNotificationAsync(Charity.Id, admins, NotificationsTypeEnum.NewCharity);

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
                Role = role,
            };

            await _unitOfWork.Donors.AddAsync(Donor);

            return result;
        }

        public async Task<LoginResponseDto> LogInAsync(LoginDto model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            UserAccount? user = new EmailAddressAttribute().IsValid(model.UserNameOrEmail)
                ? await _userManager.FindByEmailAsync(model.UserNameOrEmail)
                : await _userManager.FindByNameAsync(model.UserNameOrEmail);

            if (user == null)
                throw new InvalidOperationException("username or email are not valied");

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
                throw new UnauthorizedAccessException("password is not valied");

            var roles = await _userManager.GetRolesAsync(user);
            var jwt = await _jwtServices.CreateJwtTokenAsync(user);

            return new LoginResponseDto
            {
                Token = jwt.Token,
                Id = user.Id,
                Email = user.Email ?? "",
                UserName = user.UserName ?? "",
                Role = roles.FirstOrDefault() ?? "Donor",
            };
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
