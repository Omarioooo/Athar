using AtharPlatform.DTO;
using AtharPlatform.DTOs;
using AtharPlatform.Repositories;
using AtharPlatform.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AtharPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<UserAccount> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<UserAccount> userManager, IUnitOfWork unitOfWork,
            IAccountService accountService, IConfiguration configuration)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _accountService = accountService;
            _configuration = configuration;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DonorRegister(DonorRegisterDto modelRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(modelRequest);

                var registerResult = await _accountService.DonorRegisterAsync(modelRequest);

                if (!registerResult.Succeeded)
                    return BadRequest(registerResult.Errors);

                await _unitOfWork.SaveAsync();
                return Ok("registered Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during Register. Please try again later." });
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CharityRegister(CharityRegisterDto modelRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(modelRequest);

                var registerResult = await _accountService.CharityRegisterAsync(modelRequest);

                if (!registerResult.Succeeded)
                    return BadRequest(registerResult.Errors);

                await _unitOfWork.SaveAsync();
                return Ok("registered Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during Register. Please try again later." });
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginDto loginRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var loginResult = await _accountService.LogInAsync(loginRequest);

                if (loginResult == null)
                {
                    // Differentiate between user not found and incorrect password
                    var userExists = new EmailAddressAttribute().IsValid(loginRequest.UserNameOrEmail)
                        ? await _accountService.FindByEmailAsync(loginRequest.UserNameOrEmail)
                        : await _accountService.FindByNameAsync(loginRequest.UserNameOrEmail);

                    return Unauthorized(new
                    {
                        message = userExists == null ? "User not found" : "Invalid password"
                    });
                }

                return Ok(loginResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during login. Please try again later." });
            }
        }
    }
}