using AtharPlatform.Hubs;
using AtharPlatform.Repositories;
using AtharPlatform.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using X.Paymob.CashIn;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Text.Json;
using AtharPlatform.Dtos;
using AtharPlatform.Models;
using AtharPlatform.Models.Enum;

var builder = WebApplication.CreateBuilder(args);

// Add Controllers
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token"
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

/* Shared Db - ACTIVE */
// Inject Database    
builder.Services.AddDbContext<Context>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("connection"),
        npgsql =>
        {
            // Add basic resiliency for transient connection errors
            npgsql.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(5), errorCodesToAdd: null);
        }
    ));



/*local Db - DISABLED */
//builder.Services.AddDbContext<Context>(options =>
//    options.UseSqlServer(
//        builder.Configuration.GetConnectionString("MSSConnection"),
//        sqlServerOptions =>
//        {
//           
//            sqlServerOptions.EnableRetryOnFailure(
//                maxRetryCount: 5,
//                maxRetryDelay: TimeSpan.FromSeconds(5),
//                errorNumbersToAdd: null
//            );
//        }
//    )
//);

// Inject Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IDonorRepository, DonorRepository>();
builder.Services.AddScoped<ICharityRepository, CharityRepository>();
builder.Services.AddScoped<ICampaignRepository, CampaignRepository>();
builder.Services.AddScoped<IContentRepository, ContentRepository>();
builder.Services.AddScoped<IFollowRepository, FollowRepository>();
builder.Services.AddScoped<IReactionRepository, ReactionRepository>();
builder.Services.AddScoped<IVendorOfferRepository, VendorOfferRepository>();
builder.Services.AddScoped<IVolunteerApplicationRepository, VolunteerApplicationRepository>();


// Inject Services
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddScoped<IAccountContextService, AccountContextService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPaymentService, PaymobService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<IFollowService, FollowService>();
builder.Services.AddScoped<IReactionService, ReactionService>();
builder.Services.AddScoped<IDonationService, DonationService>();
builder.Services.AddScoped<IDonorService, DonorService>();
builder.Services.AddScoped<ICampaignService, CampaignService>();

// SignalR
builder.Services.AddSignalR();
builder.Services.AddScoped<INotificationHub, NotificationHub>();


// Identity Settings
builder.Services
    .AddIdentity<UserAccount, IdentityRole<int>>()
    .AddEntityFrameworkStores<Context>()
    .AddDefaultTokenProviders();

// Identity Password settings
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
});

// JWT Auth Settings
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey missing")))
    };
});

// CORS Settings
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(_ => true)
            .AllowCredentials();
    });
});

//Paymob Setting
//builder.Services.AddPaymobCashIn(config =>
//{
//    config.ApiKey = builder.Configuration["Paymob:ApiKey"];
//    config.IntegrationId = int.Parse(builder.Configuration["Paymob:IntegrationId"]);
//    config.Iframe = builder.Configuration["Paymob:IframeId"];
//});
builder.Services.AddHttpClient();
builder.Services.Configure<PaymentSettings>(builder.Configuration.GetSection("Paymob"));
builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(o =>
{
    o.MultipartBodyLengthLimit = long.MaxValue;
});



// Build app
Console.WriteLine("[Startup] Building web application...");
var app = builder.Build();
Console.WriteLine("[Startup] Build complete. Beginning DB init...");

// Global exception diagnostics
AppDomain.CurrentDomain.UnhandledException += (s, e) =>
{
    Console.WriteLine($"[UnhandledException] {(e.ExceptionObject is Exception ex ? ex.ToString() : e.ExceptionObject)}");
};
TaskScheduler.UnobservedTaskException += (s, e) =>
{
    Console.WriteLine($"[UnobservedTaskException] {e.Exception}");
    e.SetObserved();
};

// Host lifetime diagnostics
try
{
    var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
    lifetime.ApplicationStarted.Register(() => Console.WriteLine("[Lifetime] ApplicationStarted"));
    lifetime.ApplicationStopping.Register(() => Console.WriteLine("[Lifetime] ApplicationStopping"));
    lifetime.ApplicationStopped.Register(() => Console.WriteLine("[Lifetime] ApplicationStopped"));
}
catch (Exception lfEx)
{
    Console.WriteLine($"[Startup] Failed to wire lifetime diagnostics: {lfEx.Message}");
}

// Ensure database is created and migrations are applied at startup
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<Context>();
        var hasMigrations = db.Database.GetMigrations().Any();
        Console.WriteLine($"[Startup] Migrations detected: {hasMigrations}");
        if (hasMigrations)
        {
            Console.WriteLine("[Startup] Applying migrations...");
            db.Database.Migrate();
            Console.WriteLine("[Startup] Migrations applied.");
        }
        else
        {
            Console.WriteLine("[Startup] No migrations found. Ensuring database created...");
            db.Database.EnsureCreated();
            try
            {
                db.Database.ExecuteSqlRaw("SELECT 1 FROM [AspNetUsers]");
                Console.WriteLine("[Startup] Identity tables already exist.");
            }
            catch (Exception idEx)
            {
                Console.WriteLine($"[Startup] Identity table check failed ({idEx.Message}). Creating tables explicitly...");
                var creator = db.GetService<IRelationalDatabaseCreator>();
                creator.CreateTables();
                Console.WriteLine("[Startup] Tables created explicitly.");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Startup] Database initialization failed: {ex.Message}");
    }

    // Seed required Identity roles
    try
    {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
            var roles = new[] { "Admin", "SuperAdmin", "CharityAdmin", "Donor" };
        foreach (var r in roles)
        {
            if (!await roleManager.RoleExistsAsync(r))
            {
                var createRes = await roleManager.CreateAsync(new IdentityRole<int> { Name = r });
                Console.WriteLine(createRes.Succeeded
                    ? $"[Startup] Role '{r}' created." : $"[Startup] Failed to create role '{r}': {string.Join(',', createRes.Errors.Select(e => e.Description))}");
            }
        }

            // Ensure documented SuperAdmin user exists and is assigned role
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserAccount>>();
            const string superAdminEmail = "admin@athar.local";
            var superAdmin = await userManager.FindByEmailAsync(superAdminEmail);
            if (superAdmin == null)
            {
                superAdmin = new UserAccount
                {
                    UserName = "admin",
                    Email = superAdminEmail,
                    EmailConfirmed = true,
                    Country = "EG",
                    City = "Alexandria",
                    CreatedAt = DateTime.UtcNow
                };
                var create = await userManager.CreateAsync(superAdmin, "Admin#123");
                Console.WriteLine(create.Succeeded ? "[Startup] SuperAdmin user created." : "[Startup] Failed to create SuperAdmin user: " + string.Join(',', create.Errors.Select(e => e.Description)));
            }
            if (!await userManager.IsInRoleAsync(superAdmin, "SuperAdmin"))
            {
                var addRole = await userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
                Console.WriteLine(addRole.Succeeded ? "[Startup] SuperAdmin role assigned to admin@athar.local" : "[Startup] Failed to assign SuperAdmin role: " + string.Join(',', addRole.Errors.Select(e => e.Description)));
            }
    }
    catch (Exception rex)
    {
        Console.WriteLine($"[Startup] Role seeding failed: {rex.Message}");
    }

    // Development-only scraped data seeding (idempotent). Imports 101 charities and 23 campaigns once per local DB.
    // Runs synchronously during startup so data is guaranteed present before first HTTP request.
    try
    {
        var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
        var flag = Environment.GetEnvironmentVariable("SKIP_SCRAPED_SEED");
        if (string.IsNullOrWhiteSpace(flag) || (!flag.Equals("1", StringComparison.OrdinalIgnoreCase) && !flag.Equals("true", StringComparison.OrdinalIgnoreCase)))
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            Console.WriteLine("[Seeder] Starting scraped data import...");
            await SeedData.DevelopmentScrapedSeeder.SeedAsync(scope.ServiceProvider, env);
            Console.WriteLine($"[Seeder] Seeding completed in {sw.Elapsed.TotalSeconds:F2}s.");
        }
        else
        {
            Console.WriteLine("[Seeder] SKIP_SCRAPED_SEED set; skipping scraped data import.");
        }
    }
    catch (Exception seedEx)
    {
        Console.WriteLine($"[Seeder] Seed failed (non-fatal): {seedEx.Message}");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Always redirect HTTP->HTTPS; we will trust the dev cert locally
app.UseHttpsRedirection();
app.UseStaticFiles(); // Enable serving static files from wwwroot
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

// Map SignalR Hub
app.MapHub<NotificationHubHelper>("/notificationHub");

// Lightweight health endpoint for import script and manual checks
app.MapGet("/health", () => Results.Ok(new { status = "ok", time = DateTime.UtcNow }))
   .WithName("HealthCheck");

app.MapControllers();

try
{
    Console.WriteLine("[Startup] Starting web host...");
    app.Run();
}
catch (Exception runEx)
{
    Console.WriteLine($"[Startup] Host terminated unexpectedly: {runEx}");
}