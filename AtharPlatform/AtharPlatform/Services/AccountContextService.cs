using System.Security.Claims;

namespace AtharPlatform.Services
{
    public class AccountContextService : IAccountContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetCurrentAccountId()
        {
            // Current User
            var user = _httpContextAccessor.HttpContext?.User;


            if (user == null || !user.Identity.IsAuthenticated)
                throw new UnauthorizedAccessException("User is not authenticated");

            // Get The Id from Claim
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new Exception("User ID not found in token");

            return int.Parse(userIdClaim.Value);
        }
    }
}
