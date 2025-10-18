using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AtharPlatform.Models
{
    public class Context : IdentityDbContext<UserAccount>
    {
        public Context(DbContextOptions<Context> options) : base(options) { }
    }
}
