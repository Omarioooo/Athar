using AtharPlatform.DTO;
using AtharPlatform.Services;
using AtharPlatform.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace AtharPlatform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService<PaymobService> _paymob;
        private readonly IConfiguration _config;

        public PaymentsController(IPaymentService<PaymobService> paymob, IConfiguration config)
        {
            _paymob = paymob;
            _config = config;
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> WebHook([FromForm] Dictionary<string, string> form)
        {
            // Check HMAC
            // if not a test mode we should check the HMAC

            // Call Verification func to update states


            return Ok();
        }

    }
}
