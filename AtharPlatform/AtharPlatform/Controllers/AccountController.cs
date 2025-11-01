using AtharPlatform.DTO;
using AtharPlatform.DTOs;
using AtharPlatform.Models.Enums;
using AtharPlatform.Repositories;
using AtharPlatform.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data;

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
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(model + "Model is not valid");

                var registerResult = await _accountService.PersonRegisterAsync(model, RolesEnum.Donor);

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
        public async Task<IActionResult> AdminRegister([FromForm] PersonRegisterDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(model + "Model is not valid");

                var registerResult = await _accountService.PersonRegisterAsync(model, RolesEnum.Admin);

                if (!registerResult.Succeeded)
                    return BadRequest(registerResult.Errors);

                await _unitOfWork.SaveAsync();
                return Ok("registered Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CharityRegister([FromForm] CharityRegisterDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(model);

                var registerResult = await _accountService.CharityRegisterAsync(model);

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
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var loginResult = await _accountService.LogInAsync(model);

                if (loginResult == null)
                {
                    // Differentiate between user not found and incorrect password
                    var userExists = new EmailAddressAttribute().IsValid(model.UserNameOrEmail)
                        ? await _accountService.FindByEmailAsync(model.UserNameOrEmail)
                        : await _accountService.FindByNameAsync(model.UserNameOrEmail);

                    return Unauthorized(new
                    {
                        message = userExists == null ? "User not found" : "Invalid password"
                    });
                }
                return Ok(loginResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}