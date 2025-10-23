
using AtharPlatform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AtharPlatform
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            // Inject DB
            /*builder.Services.AddDbContext<Context>(op =>
                op.UseNpgsql(builder.Configuration.GetConnectionString("connection")
            ));*/

            builder.Services.AddDbContext<Context>(
              options => options.UseNpgsql(builder.Configuration.GetConnectionString("connection"))
              );

            // Inject Identity with EF stores
            builder.Services
                .AddIdentity<UserAccount, IdentityRole>()
                .AddEntityFrameworkStores<Context>()
                .AddDefaultTokenProviders();

            var app = builder.Build();

            // Seed required Identity roles on first run
            try
            {
                using var scope = app.Services.CreateScope();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                string[] roles = new[] { "SuperAdmin", "CharityAdmin", "Donor", "Vendor" };
                foreach (var role in roles)
                {
                    var exists = roleManager.RoleExistsAsync(role).GetAwaiter().GetResult();
                    if (!exists)
                    {
                        roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
                    }
                }
            }
            catch
            {
                // Swallow seeding errors so dev startup doesn't crash if DB is unavailable
                // Consider adding proper logging here.
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}