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
        private readonly UserManager<UserAccount> _userManager;
        private readonly IConfiguration _configuration;

        public AccountService(IUnitOfWork unitOfWork,
            UserManager<UserAccount> userManager, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<IdentityResult> CharityRegisterAsync(CharityRegisterDto model)
        {
            try
            {
                if (model == null)
                    return IdentityResult.Failed(new IdentityError { Description = "Invalid registration data." });

                if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
                    return IdentityResult.Failed(new IdentityError { Description = "Email and password are required." });

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
                    return result;

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
            catch
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Invalid registration data."
                });
            }
        }

        public async Task<IdentityResult> PersonRegisterAsync(PersonRegisterDto model, RolesEnum role)
        {
            try
            {
                if (model == null)
                    return IdentityResult.Failed(new IdentityError { Description = "Invalid registration data." });

                if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
                    return IdentityResult.Failed(new IdentityError { Description = "Email and password are required." });

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
                    return result;

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
            catch
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Invalid registration data."
                });
            }
        }

        public async Task<object?> LogInAsync(LoginDto model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.UserNameOrEmail) || string.IsNullOrWhiteSpace(model.Password))
                return null;

            UserAccount? user;
            string userNameOrEmail = model.UserNameOrEmail.Trim();

            if (new EmailAddressAttribute().IsValid(userNameOrEmail))
            {
                user = await _userManager.FindByEmailAsync(userNameOrEmail.ToLowerInvariant());
            }
            else
            {
                user = await _userManager.FindByNameAsync(userNameOrEmail);
            }

            if (user == null)
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Invalid Username or Email."
                });

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Invalid Password."
                });

            // Rest of the method remains unchanged
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var secretKey = _configuration["Jwt:SecretKey"];
            if (string.IsNullOrEmpty(secretKey) || Encoding.UTF8.GetBytes(secretKey).Length < 32)
                throw new InvalidOperationException("JWT SecretKey is missing or too short. It must be at least 32 bytes.");

            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
                throw new InvalidOperationException("JWT Issuer or Audience is missing.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                issuer: issuer,
                audience: audience,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new
            {
                message = "Login succeeded",
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            };
        }

        public async Task<UserAccount> FindByEmailAsync(string mail)
        {
            return await _userManager.FindByEmailAsync(mail);
        }

        public async Task<UserAccount> FindByNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }
    }
}
