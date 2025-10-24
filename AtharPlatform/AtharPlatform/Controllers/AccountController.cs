using AtharPlatform.DTO;
using AtharPlatform.Repositories;
using AtharPlatform.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Register(RegisterDto registerRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(registerRequest);

            var registerResult = await _accountService.RegisterAsync(registerRequest);

            if (!registerResult.Succeeded)
                return BadRequest(registerResult.Errors);

            await _unitOfWork.SaveAsync();
            return Ok("registered Successfully");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginDto loginRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(loginRequest);

            var token = _accountService.LogInAsync(loginRequest);
            if (token == null)
                return Unauthorized("Invalid username or password");

            await _unitOfWork.SaveAsync();

            return Ok(token);
        }
    }
}