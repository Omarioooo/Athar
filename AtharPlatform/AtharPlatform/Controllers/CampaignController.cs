using AtharPlatform.Dtos;
using AtharPlatform.Models.Enum;
using AtharPlatform.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace AtharPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CampaignController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        //public async Task<IActionResult> GetAllCampaign()
        //{
        //    var campaign = await _icampaignrepository.GetAllAsync();
        //    var result = campaign.Select(c => new CampaignDto
        //    {
        //        Id = c.Id,
        //        Title = c.Title,
        //        Description = c.Description,
        //        Image = c.Image,
        //        GoalAmount = c.GoalAmount,
        //        RaisedAmount = c.RaisedAmount,
        //        StartDate = c.StartDate,
        //        EndDate = c.EndDate,
        //        Category = c.Category,
        //        CharityName = c.Charity.Name


        //    });
        //    return Ok(result);

        //}
        [HttpGet("[action]")]
        public async Task<ActionResult<PaginatedResultDto<CampaignDto>>> GetAll([FromQuery] string? query, [FromQuery] int page = 1, [FromQuery] int pageSize = 12)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest("Page and PageSize must be greater than zero.");

            var total = await _unitOfWork.Campaign.CountAsync(query);

            if (total == 0)
            {
                return Ok(new PaginatedResultDto<CampaignDto>
                {
                    Items = new List<CampaignDto>(),
                    Page = page,
                    PageSize = pageSize,
                    Total = 0
                });
            }

            var campaigns = await _unitOfWork.Campaign.GetPageAsync(query, page, pageSize);

            var dto = new PaginatedResultDto<CampaignDto>
            {
                Items = campaigns.Select(c => new CampaignDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    ImageUrl = c.ImageUrl,
                    GoalAmount = c.GoalAmount,
                    RaisedAmount = c.RaisedAmount,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    Category = c.Category,
                    CharityName = c.Charity?.Name
                }),
                Page = page,
                PageSize = pageSize,
                Total = total
            };

            return Ok(dto);
        }


        [HttpGet("[action]")]
        public async Task<IActionResult>GetCampaignById(int id)
        {
            var campaign = await _unitOfWork.Campaign.GetAsync(id);
            if (campaign == null)
                return NotFound("Campaign not found.");
            var result = new CampaignDto
            {
                Id = campaign.Id,
                Title = campaign.Title,
                Description = campaign.Description,
                ImageUrl = campaign.ImageUrl,
                GoalAmount = campaign.GoalAmount,
                RaisedAmount = campaign.RaisedAmount,
                StartDate = campaign.StartDate,
                EndDate = campaign.EndDate,
                Category = campaign.Category,
                CharityName = campaign.Charity?.Name
            };
            return Ok(result);

        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetByType(CampaignCategoryEnum type)
        {
            var campaign = await _unitOfWork.Campaign.GetByType(type);
            if (campaign == null || !campaign.Any())// it return list so I check if list is Empty
                return NotFound("Campaign not found.");
            var result = campaign.Select(c => new CampaignDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                GoalAmount = c.GoalAmount,
                RaisedAmount = c.RaisedAmount,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                Category = c.Category,
                CharityName = c.Charity.Name
            });
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetByDate([FromQuery] bool latestFirst = true)
        {
            var campaigns = await _unitOfWork.Campaign.GetByDateAsync(latestFirst);

            if (campaigns == null || !campaigns.Any())
                return NotFound("No campaigns found.");

            var result = campaigns.Select(c => new CampaignDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                GoalAmount = c.GoalAmount,
                RaisedAmount = c.RaisedAmount,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                Category = c.Category,
                CharityName = c.Charity?.Name
            });

            return Ok(result);
        }



        /// <summary>
        /// Bulk import campaigns (SuperAdmin only). Links to the first matching existing charity by name from supporting_charities or charity_name.
        /// Missing numeric/date fields will be randomized; duplicates (same Title+Charity) will be skipped.
        /// </summary>
        [HttpPost("import")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ImportCampaigns([FromBody] List<CampaignImportItemDto> items)
        {
            if (items == null || items.Count == 0)
                return BadRequest(new { error = "Empty payload" });

            int imported = 0, skipped = 0, withoutCharity = 0, duplicates = 0, invalid = 0;

            foreach (var i in items)
            {
                try
                {
                    // Basic validation
                    var title = (i.Title ?? string.Empty).Trim();
                    var description = (i.Description ?? string.Empty).Trim();
                    var imageUrl = (i.ImageUrl ?? string.Empty).Trim();

                    if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description) || string.IsNullOrWhiteSpace(imageUrl))
                    {
                        invalid++;
                        continue;
                    }

                    // Resolve charity by provided charity_name first, else supporting_charities
                    var charityNames = new List<string>();
                    if (!string.IsNullOrWhiteSpace(i.CharityName)) charityNames.Add(i.CharityName!.Trim());
                    if (i.SupportingCharities != null) charityNames.AddRange(i.SupportingCharities.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()));

                    Charity? charity = null;
                    foreach (var name in charityNames.Distinct())
                    {
                        var n = name;
                        charity = await _unitOfWork.Charity.GetAsync(c => c.Name == n || EF.Functions.Like(c.Name, $"%{n}%"));
                        if (charity != null) break;
                    }

                    if (charity == null)
                    {
                        // Fallback: assign randomly from existing 101 charities (current DB state)
                        var allCharities = await _unitOfWork.Charity.GetAllAsync();
                        if (allCharities == null || allCharities.Count == 0)
                        {
                            withoutCharity++;
                            continue;
                        }
                        charity = allCharities[Random.Shared.Next(0, allCharities.Count)];
                    }

                    // Check duplicate by Title+Charity
                    var exists = await _unitOfWork.Campaign.GetAsync(c => c.Title == title && c.CharityID == charity.Id);
                    if (exists != null)
                    {
                        duplicates++;
                        continue;
                    }

                    // Randomize missing values
                    var rng = Random.Shared;
                    var startDate = i.StartDate ?? DateTime.UtcNow.Date.AddDays(-rng.Next(0, 60));
                    var duration = i.DurationDays ?? rng.Next(20, 61); // 20-60 days
                    var goal = i.GoalAmount ?? Math.Round(rng.Next(50_000, 500_001) / 100.0) * 100; // round to nearest 100
                    var raised = i.RaisedAmount ?? Math.Round(goal * (rng.Next(20, 81) / 100.0), 2);
                    var isCritical = i.IsCritical ?? false;

                    // Infer category
                    var category = InferCategory(i.Category, title, description);

                    // Determine status
                    var status = CampainStatusEnum.inProgress;
                    if (raised >= goal) status = CampainStatusEnum.Completed;
                    else if (DateTime.UtcNow.Date > startDate.AddDays(duration)) status = CampainStatusEnum.expired;

                    var campaign = new Campaign
                    {
                        Title = title,
                        Description = description,
                        ImageUrl = imageUrl,
                        StartDate = startDate,
                        Duration = duration,
                        GoalAmount = goal,
                        RaisedAmount = raised,
                        isCritical = isCritical,
                        Category = category,
                        Status = status,
                        CharityID = charity.Id
                    };

                    await _unitOfWork.Campaign.AddAsync(campaign);
                    imported++;
                }
                catch
                {
                    skipped++;
                }
            }

            await _unitOfWork.SaveAsync();

            return Ok(new { imported, skipped, withoutCharity, duplicates, invalid });
        }

        private static CampaignCategoryEnum InferCategory(string? provided, string title, string description)
        {
            if (!string.IsNullOrWhiteSpace(provided))
            {
                var p = provided.Trim().ToLowerInvariant();
                if (p.Contains("health") || p.Contains("صحة") || p.Contains("طبي")) return CampaignCategoryEnum.Health;
                if (p.Contains("education") || p.Contains("تعليم")) return CampaignCategoryEnum.Education;
                if (p.Contains("food") || p.Contains("غذاء") || p.Contains("طعام") || p.Contains("سلة") || p.Contains("إطعام")) return CampaignCategoryEnum.Food;
                if (p.Contains("shelter") || p.Contains("مأوى") || p.Contains("سكن")) return CampaignCategoryEnum.Shelter;
                if (p.Contains("يتيم") || p.Contains("الأيتام") || p.Contains("orphans")) return CampaignCategoryEnum.Orphans;
            }

            var t = $"{title} {description}".ToLowerInvariant();
            if (t.Contains("صحة") || t.Contains("علاج") || t.Contains("طبي") || t.Contains("دواء") || t.Contains("مريض")) return CampaignCategoryEnum.Health;
            if (t.Contains("تعليم") || t.Contains("مدرسة") || t.Contains("طلاب") || t.Contains("جامع")) return CampaignCategoryEnum.Education;
            if (t.Contains("غذاء") || t.Contains("طعام") || t.Contains("سلة") || t.Contains("إطعام")) return CampaignCategoryEnum.Food;
            if (t.Contains("مأوى") || t.Contains("سكن") || t.Contains("الفقراء") || t.Contains("خيمة") || t.Contains("لاجئ")) return CampaignCategoryEnum.Shelter;
            if (t.Contains("يتيم") || t.Contains("أيتام") || t.Contains("كفالة")) return CampaignCategoryEnum.Orphans;
            if (t.Contains("إغاثة") || t.Contains("اغاثة") || t.Contains("الإغاثة العاجلة")) return CampaignCategoryEnum.Other; // could be a separate enum later
            return CampaignCategoryEnum.Other;
        }


    }
}
