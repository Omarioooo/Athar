using System.Text.Json;
using AtharPlatform.Dtos;
using AtharPlatform.Models;
using AtharPlatform.Models.Enum;
using AtharPlatform.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SeedData
{
    public static class DevelopmentScrapedSeeder
    {
        public static async Task SeedAsync(IServiceProvider services, IWebHostEnvironment env)
        {
            // Only run in Development
            if (!env.IsDevelopment()) return;

            // Global skip flag to allow fast startup when desired
            var skipFlag = Environment.GetEnvironmentVariable("SKIP_SCRAPED_SEED");
            if (!string.IsNullOrWhiteSpace(skipFlag) && (skipFlag.Equals("1", StringComparison.OrdinalIgnoreCase) || skipFlag.Equals("true", StringComparison.OrdinalIgnoreCase)))
                return;

            var logger = services.GetService<ILoggerFactory>()?.CreateLogger("DevScrapedSeeder");
            using var scope = services.CreateScope();
            var sp = scope.ServiceProvider;

            var db = sp.GetRequiredService<Context>();
            var uow = sp.GetRequiredService<IUnitOfWork>();
            var userManager = sp.GetRequiredService<UserManager<UserAccount>>();

            // Quick idempotency guard — if we already have close to the full dataset, skip.
            var charitiesCount = await db.Charities.CountAsync(c => c.IsScraped);
            var campaignsCount = await db.Campaigns.CountAsync();
            if (charitiesCount >= 100 && campaignsCount >= 20)
            {
                logger?.LogInformation("[Seeder] Skipping: scraped data already present (charities={Charities}, campaigns={Campaigns}).", charitiesCount, campaignsCount);
                return;
            }

            var root = env.ContentRootPath;
            var charitiesPath = Path.Combine(root, "SeedData", "charities101.json");
            var campaignsPath = Path.Combine(root, "SeedData", "campagins23.json");

            if (charitiesCount < 100 && File.Exists(charitiesPath))
            {
                logger?.LogInformation("[Seeder] Importing charities from {Path}...", charitiesPath);
                var json = await File.ReadAllTextAsync(charitiesPath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var items = JsonSerializer.Deserialize<List<CharityImportItemDto>>(json, options) ?? new();
                var now = DateTime.UtcNow;

                var entities = new List<Charity>(items.Count);
                var addedCount = 0;
                var skippedCount = 0;
                var sw = System.Diagnostics.Stopwatch.StartNew();
                foreach (var i in items)
                {
                    var safeName = (i.Name ?? string.Empty).Trim();
                    if (string.IsNullOrWhiteSpace(safeName))
                        continue;

                    // Idempotency: skip if a scraped charity already exists by ExternalId or exact Name
                    Charity? existing = null;
                    if (!string.IsNullOrWhiteSpace(i.ExternalId))
                        existing = await db.Charities.FirstOrDefaultAsync(c => c.IsScraped && c.ExternalId == i.ExternalId);
                    if (existing == null)
                        existing = await db.Charities.FirstOrDefaultAsync(c => c.IsScraped && c.Name == safeName);
                    if (existing != null)
                    {
                        skippedCount++;
                        continue;
                    }
                    // Create placeholder Identity account to satisfy FK (Charity.Id == UserAccount.Id)
                    var unameBase = (safeName.Length > 0 ? safeName : $"charity_{Guid.NewGuid():N}").Replace(" ", "_");
                    var uname = unameBase;
                    var email = $"{uname.ToLowerInvariant()}@charity.local";
                    var account = new UserAccount { UserName = uname, Email = email, EmailConfirmed = true };
                    var pwd = $"{Guid.NewGuid():N}!Aa1";

                    var createRes = await userManager.CreateAsync(account, pwd);
                    if (!createRes.Succeeded)
                    {
                        uname = $"{unameBase}_{Guid.NewGuid().ToString("N")[..6]}";
                        email = $"{uname.ToLowerInvariant()}@charity.local";
                        account = new UserAccount { UserName = uname, Email = email, EmailConfirmed = true };
                        createRes = await userManager.CreateAsync(account, pwd);
                        if (!createRes.Succeeded)
                        {
                            logger?.LogWarning("[Seeder] Skipping charity '{Name}' due to identity creation errors: {Err}", safeName, string.Join(';', createRes.Errors.Select(e => e.Description)));
                            continue;
                        }
                    }

                    var charity = new Charity
                    {
                        Id = account.Id,
                        Account = account,
                        Name = safeName,
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

                    entities.Add(charity);
                    addedCount++;
                }

                if (entities.Count > 0)
                {
                    await uow.Charities.BulkImportAsync(entities);
                    await uow.SaveAsync();
                    logger?.LogInformation("[Seeder] Imported {Added} charities (skipped {Skipped}) in {Elapsed:n1}s.", addedCount, skippedCount, sw.Elapsed.TotalSeconds);
                }
                else
                {
                    logger?.LogInformation("[Seeder] No new charity entities to import (skipped {Skipped}).", skippedCount);
                }
            }
            else if (!File.Exists(charitiesPath))
            {
                logger?.LogWarning("[Seeder] Charities JSON not found at {Path}", charitiesPath);
            }

            // Refresh counts after charity import to ensure campaign linkage can succeed
            charitiesCount = await db.Charities.CountAsync(c => c.IsScraped);

            if (campaignsCount < 20 && File.Exists(campaignsPath) && charitiesCount > 0)
            {
                logger?.LogInformation("[Seeder] Importing campaigns from {Path}...", campaignsPath);
                var json = await File.ReadAllTextAsync(campaignsPath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var items = JsonSerializer.Deserialize<List<CampaignImportItemDto>>(json, options) ?? new();
                var now = DateTime.UtcNow;

                var added = 0;
                var skipped = 0;
                var swC = System.Diagnostics.Stopwatch.StartNew();
                foreach (var i in items)
                {
                    if (string.IsNullOrWhiteSpace(i?.Title) || string.IsNullOrWhiteSpace(i?.Description))
                        continue;

                    int charityId = 0;
                    string? nameHint = i.CharityName;
                    if (string.IsNullOrWhiteSpace(nameHint) && (i.SupportingCharities?.Any() ?? false))
                        nameHint = i.SupportingCharities!.FirstOrDefault();

                    if (!string.IsNullOrWhiteSpace(nameHint))
                    {
                        var nm = nameHint.Trim();
                        try
                        {
                            var charity = await uow.Charities.GetWithExpressionAsync(c => c.IsScraped && (c.Name == nm || c.Name.Contains(nm)));
                            if (charity != null) charityId = charity.Id;
                        }
                        catch { /* ignore lookup failures */ }
                    }

                    if (charityId == 0) continue; // cannot link campaign without a charity

                    var cat = CampaignCategoryEnum.Other;
                    if (!string.IsNullOrWhiteSpace(i.Category) && Enum.TryParse<CampaignCategoryEnum>(i.Category, true, out var parsed))
                        cat = parsed;

                    // Idempotency: skip if campaign already exists by ExternalId or (Title+CharityID)
                    if (!string.IsNullOrWhiteSpace(i.ExternalId))
                    {
                        var existsByExt = await db.Campaigns.AnyAsync(c => c.ExternalId == i.ExternalId && c.CharityID == charityId);
                        if (existsByExt) { skipped++; continue; }
                    }
                    else
                    {
                        var t = i.Title!.Trim();
                        var existsByKey = await db.Campaigns.AnyAsync(c => c.CharityID == charityId && c.Title == t);
                        if (existsByKey) { skipped++; continue; }
                    }

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
                        CharityID = charityId,
                        ExternalId = i.ExternalId,
                        SupportingCharitiesJson = (i.SupportingCharities != null && i.SupportingCharities.Any()) ? JsonSerializer.Serialize(i.SupportingCharities) : null
                    };

                    await uow.Campaigns.AddAsync(campaign);
                    added++;
                }

                if (added > 0)
                {
                    await uow.SaveAsync();
                    logger?.LogInformation("[Seeder] Imported {Count} campaigns (skipped {Skipped}) in {Elapsed:n1}s.", added, skipped, swC.Elapsed.TotalSeconds);
                }
                else
                {
                    logger?.LogInformation("[Seeder] No new campaigns imported (skipped {Skipped}).", skipped);
                }
            }
            else if (!File.Exists(campaignsPath))
            {
                logger?.LogWarning("[Seeder] Campaigns JSON not found at {Path}", campaignsPath);
            }
        }
    }
}
