using AtharPlatform.DTO;
using AtharPlatform.Models.Enums;
using AtharPlatform.Repositories;
using AtharPlatform.Services;
using Microsoft.AspNetCore.Mvc;


namespace AtharPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountService _accountService;

        public AccountController(IUnitOfWork unitOfWork, IAccountService accountService)
        {
            _unitOfWork = unitOfWork;
            _accountService = accountService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DonorRegister([FromForm] PersonRegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid registration data." });

            try
            {
                var registerResult = await _accountService.PersonRegisterAsync(model, RolesEnum.Donor);

                if (!registerResult.Succeeded)
                    return BadRequest(new { message = "Registration failed.", errors = registerResult.Errors });

                await _unitOfWork.SaveAsync();
                return Ok(new { message = "Donor registered successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred during donor registration." });
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AdminRegister([FromForm] PersonRegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid registration data." });

            try
            {
                var registerResult = await _accountService.PersonRegisterAsync(model, RolesEnum.Admin);


                if (!registerResult.Succeeded)
                    return BadRequest(new { message = "Registration failed.", errors = registerResult.Errors });

                await _unitOfWork.SaveAsync();
                return Ok(new { message = "Admin registered successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred during admin registration." });
            }
        }

        // JSON-friendly variant for automation (PowerShell 5.1 lacks -Form)
        [HttpPost("AdminRegisterJson")]
        public async Task<IActionResult> AdminRegisterJson([FromBody] PersonRegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid registration data." });

            try
            {
                var registerResult = await _accountService.PersonRegisterAsync(model, RolesEnum.Admin);

                if (!registerResult.Succeeded)
                    return BadRequest(new { message = "Registration failed.", errors = registerResult.Errors });

                await _unitOfWork.SaveAsync();
                return Ok(new { message = "Admin registered successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred during admin registration." });
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CharityRegister([FromForm] CharityRegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid registration data." });

            try
            {
                var registerResult = await _accountService.CharityRegisterAsync(model);

                if (!registerResult.Succeeded)
                    return BadRequest(new { message = "Registration failed.", errors = registerResult.Errors });

                await _unitOfWork.SaveAsync();
                return Ok(new { message = "Charity registered successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred during charity registration." });
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid login data." });

            try
            {
                var loginResult = await _accountService.LogInAsync(model);
                return Ok(loginResult);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An unexpected error occurred during login." });
            }
        }
    }
}