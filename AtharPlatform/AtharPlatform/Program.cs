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
builder.Services.AddSwaggerGen();

// Inject Database
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("MSSConnection"),
        sql =>
        {
            // Add basic resiliency for transient SQL errors
            sql.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: null);
        }
    ));

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
builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddScoped<IAccountContextService, AccountContextService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPaymentService<PaymobService>, PaymobService>();
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
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
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

// Paymob Setting
builder.Services.AddPaymobCashIn(config =>
{
    config.ApiKey = builder.Configuration["Paymob:ApiKey"];
    config.Hmac = builder.Configuration["Paymob:Hmac"];
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
    }
    catch (Exception rex)
    {
        Console.WriteLine($"[Startup] Role seeding failed: {rex.Message}");
    }

    // Seed scraped data from JSON files on first run (idempotent)
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<Context>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserAccount>>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var contentRoot = app.Environment.ContentRootPath;
        var seedDir = Path.Combine(contentRoot, "SeedData");
        Directory.CreateDirectory(seedDir);

        int charityCount = await db.Charities.CountAsync();
        int campaignCount = await db.Campaigns.CountAsync();

        // Helper to read first matching JSON file by keyword
        string? FindJson(string keyword)
        {
            if (!Directory.Exists(seedDir)) return null;
            var files = Directory.EnumerateFiles(seedDir, "*.json", SearchOption.TopDirectoryOnly)
                .Where(f => Path.GetFileName(f).Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return files.FirstOrDefault();
        }

        if (charityCount < 100)
        {
            var charitiesPath = FindJson("charit");
            if (charitiesPath != null)
            {
                Console.WriteLine($"[Seed] Importing charities from {Path.GetFileName(charitiesPath)} ...");
                var json = await File.ReadAllTextAsync(charitiesPath);
                var items = JsonSerializer.Deserialize<List<CharityImportItemDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
                var now = DateTime.UtcNow;
                var toAdd = new List<Charity>();
                foreach (var i in items.Where(i => !string.IsNullOrWhiteSpace(i.Name)))
                {
                    // Use safe ASCII-only usernames/emails regardless of charity display name (Arabic allowed in Charity.Name)
                    var uname = $"charity_{Guid.NewGuid():N}";
                    var email = $"{uname}@charity.local";
                    var account = new UserAccount { UserName = uname, Email = email, EmailConfirmed = true };
                    var pwd = $"{Guid.NewGuid():N}!Aa1";
                    var res = await userManager.CreateAsync(account, pwd);
                    if (!res.Succeeded) { continue; }

                    var charity = new Charity
                    {
                        Id = account.Id,
                        Account = account,
                        Name = i.Name.Trim(),
                        Description = i.Description ?? string.Empty,
                        IsScraped = true,
                        ExternalId = i.ExternalId,
                        ImportedAt = now,
                        VerificationDocument = Array.Empty<byte>()
                    };
                    if (i.ImageUrl != null || i.ExternalWebsiteUrl != null)
                    {
                        charity.ScrapedInfo = new CharityExternalInfo
                        {
                            ImageUrl = i.ImageUrl,
                            ExternalWebsiteUrl = i.ExternalWebsiteUrl
                        };
                    }
                    toAdd.Add(charity);
                }
                if (toAdd.Count > 0)
                {
                    await unitOfWork.Charities.BulkImportAsync(toAdd);
                    await unitOfWork.SaveAsync();
                    Console.WriteLine($"[Seed] Imported {toAdd.Count} charities.");
                    charityCount += toAdd.Count;
                }
            }
            else
            {
                Console.WriteLine($"[Seed] No charities JSON found in {seedDir}. Skipping charity seed.");
            }
        }

        if (campaignCount < 23)
        {
            var campaignsPath = FindJson("campagin") ?? FindJson("campaign");
            if (campaignsPath != null)
            {
                Console.WriteLine($"[Seed] Importing campaigns from {Path.GetFileName(campaignsPath)} ...");
                var json = await File.ReadAllTextAsync(campaignsPath);
                var items = JsonSerializer.Deserialize<List<CampaignImportItemDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
                var now = DateTime.UtcNow;
                var added = 0;
                foreach (var i in items)
                {
                    if (string.IsNullOrWhiteSpace(i?.Title) || string.IsNullOrWhiteSpace(i?.Description)) continue;
                    string? nameHint = i.CharityName;
                    if (string.IsNullOrWhiteSpace(nameHint) && (i.SupportingCharities?.Any() ?? false))
                        nameHint = i.SupportingCharities!.FirstOrDefault();
                    if (string.IsNullOrWhiteSpace(nameHint)) continue;

                    var nm = nameHint.Trim();
                    Charity? charity = null;
                    try
                    {
                        charity = await unitOfWork.Charities.GetWithExpressionAsync(c => c.IsScraped && (c.Name == nm || c.Name.Contains(nm)));
                    }
                    catch { }
                    if (charity == null) continue;

                    var cat = CampaignCategoryEnum.Other;
                    if (!string.IsNullOrWhiteSpace(i.Category) && Enum.TryParse<CampaignCategoryEnum>(i.Category, true, out var parsed))
                        cat = parsed;

                    var campaign = new Campaign
                    {
                        Title = i.Title!.Trim(),
                        Description = i.Description!.Trim(),
                        ImageUrl = i.ImageUrl,
                        isCritical = i.IsCritical ?? false,
                        StartDate = i.StartDate ?? now,
                        Duration = i.DurationDays ?? 30,
                        GoalAmount = i.GoalAmount ?? 0,
                        RaisedAmount = i.RaisedAmount ?? 0,
                        IsInKindDonation = false,
                        Category = cat,
                        Status = CampainStatusEnum.inProgress,
                        CharityID = charity.Id,
                        ExternalId = i.ExternalId,
                        SupportingCharitiesJson = (i.SupportingCharities != null && i.SupportingCharities.Any()) ? JsonSerializer.Serialize(i.SupportingCharities) : null
                    };

                    await unitOfWork.Campaigns.AddAsync(campaign);
                    added++;
                }
                if (added > 0)
                {
                    await unitOfWork.SaveAsync();
                    Console.WriteLine($"[Seed] Imported {added} campaigns.");
                }
            }
            else
            {
                Console.WriteLine($"[Seed] No campaigns JSON found in {seedDir}. Skipping campaign seed.");
            }
        }
    }
    catch (Exception seedEx)
    {
        Console.WriteLine($"[Seed] Seeding failed: {seedEx.Message}");
    }
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
// Temporarily disable HTTPS redirection to simplify local health checks
// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Map SignalR Hub
app.MapHub<NotificationHubHelper>("/notificationHub");

// Lightweight health endpoint for import script and manual checks
app.MapGet("/health", () => Results.Ok(new { status = "ok", time = DateTime.UtcNow }))
   .WithName("HealthCheck");

app.MapControllers();
Console.WriteLine("[Startup] Starting web host...");
try
{
    app.Run();
}
catch (Exception runEx)
{
    Console.WriteLine($"[Startup] Host terminated unexpectedly: {runEx.Message}");
    throw;
}