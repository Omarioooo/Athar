using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;

namespace AtharPlatform.DTOs
{
    public class TokenDto
    {
        public string Message { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
