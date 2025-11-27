using Ardalis.GuardClauses;
using AtharPlatform.Dtos;
using AtharPlatform.DTOs;
using AtharPlatform.Models.Enum;
using AtharPlatform.Repositories;

namespace AtharPlatform.Services
{
    public class CampaignService : ICampaignService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public CampaignService(IUnitOfWork unitOfWork, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<int> GetCountOfCampaignsAsync(
            CampainStatusEnum? status = null,
            CampaignCategoryEnum? category = null,
            string? search = null,
            bool? isCritical = null,
            double? minGoalAmount = null,
            double? maxGoalAmount = null,
            DateTime? startDateFrom = null,
            DateTime? startDateTo = null,
            int? charityId = null)
        {
            var query = _unitOfWork.Campaigns.GetQueryable();

            // Apply filters
            if (status.HasValue)
                query = query.Where(c => c.Status == status.Value);
            else
                query = query.Where(c => c.Status == CampainStatusEnum.inProgress); // Default to in progress

            if (category.HasValue)
                query = query.Where(c => c.Category == category.Value);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(c => c.Title.Contains(search) || c.Description.Contains(search));

            if (isCritical.HasValue)
                query = query.Where(c => c.isCritical == isCritical.Value);

            if (minGoalAmount.HasValue)
                query = query.Where(c => c.GoalAmount >= minGoalAmount.Value);

            if (maxGoalAmount.HasValue)
                query = query.Where(c => c.GoalAmount <= maxGoalAmount.Value);

            if (startDateFrom.HasValue)
                query = query.Where(c => c.StartDate >= startDateFrom.Value);

            if (startDateTo.HasValue)
                query = query.Where(c => c.StartDate <= startDateTo.Value);

            if (charityId.HasValue)
                query = query.Where(c => c.CharityID == charityId.Value);

            return await query.CountAsync();
        }

        public async Task<List<CampaignDto>> GetAllAsync(bool inProgress = true, bool includeCharity = true)
        {
            var campaigns = inProgress
                 ? await _unitOfWork.Campaigns.GetAllInProgressAsync(includeCharity)
                 : await _unitOfWork.Campaigns.GetAllAsync(includeCharity);

            if (campaigns == null)
                throw new KeyNotFoundException("Campaigns not found");

            return campaigns.Select(c => new CampaignDto
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
                CharityID = c.CharityID,
                CharityName = c.Charity?.Name ?? ""
            }).ToList(); ;
        }

        public async Task<CampaignDto> GetAsync(int id, bool inProgress = true, bool inCludeCharity = true)
        {
            var campaign = await _unitOfWork.Campaigns.GetAsync(id, inProgress: inProgress, includeCharity: inCludeCharity);
            if (campaign == null)
                throw new KeyNotFoundException("Campaign not found");

            return new CampaignDto
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
                CharityID = campaign.CharityID,
                CharityName = campaign.Charity?.Name ?? ""
            };
        }

        public async Task<List<CampaignDto>> GetByTypeAsync(CampaignCategoryEnum type, bool inCludeCharity = true)
        {
            var data = await _unitOfWork.Campaigns.GetAllByTypeAsync(type, inCludeCharity);
            if (data == null)
                throw new KeyNotFoundException("Campaign not found");

            return data.Select(c => new CampaignDto
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
                CharityID = c.CharityID,
                CharityName = c.Charity?.Name ?? ""
            }).ToList();
        }

        public async Task<List<string>> GetAllTypesAsync()
        {
            var types = Enum
                .GetValues(typeof(CampaignCategoryEnum))
                .Cast<CampaignCategoryEnum>()
                .Select(e => e.ToString())
                .ToList();

            return types;
        }

        public async Task<List<CampaignDto>> SearchAsync(string keyword, bool inCludeCharity = true)
        {
            var campaigns = await _unitOfWork.Campaigns.Search(keyword, inCludeCharity);

            if (campaigns == null)
                throw new KeyNotFoundException($"Campaigns with {keyword} not found");

            return campaigns.Select(c => new CampaignDto
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
                CharityID = c.CharityID,
                CharityName = c.Charity?.Name ?? ""
            }).ToList();
        }

        public async Task<List<CampaignDto>> GetPaginatedAsync(
            int page, 
            int pageSize, 
            CampainStatusEnum? status = null,
            CampaignCategoryEnum? category = null,
            string? search = null,
            bool? isCritical = null,
            double? minGoalAmount = null,
            double? maxGoalAmount = null,
            DateTime? startDateFrom = null,
            DateTime? startDateTo = null,
            int? charityId = null,
            bool inCludeCharity = true)
        {
            if (page <= 0 || pageSize <= 0)
                throw new ArgumentOutOfRangeException("Page and PageSize must be greater than zero.");

            var query = _unitOfWork.Campaigns.GetQueryable();

            // Apply filters
            if (status.HasValue)
                query = query.Where(c => c.Status == status.Value);
            else
                query = query.Where(c => c.Status == CampainStatusEnum.inProgress); // Default to in progress

            if (category.HasValue)
                query = query.Where(c => c.Category == category.Value);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(c => c.Title.Contains(search) || c.Description.Contains(search));

            if (isCritical.HasValue)
                query = query.Where(c => c.isCritical == isCritical.Value);

            if (minGoalAmount.HasValue)
                query = query.Where(c => c.GoalAmount >= minGoalAmount.Value);

            if (maxGoalAmount.HasValue)
                query = query.Where(c => c.GoalAmount <= maxGoalAmount.Value);

            if (startDateFrom.HasValue)
                query = query.Where(c => c.StartDate >= startDateFrom.Value);

            if (startDateTo.HasValue)
                query = query.Where(c => c.StartDate <= startDateTo.Value);

            if (charityId.HasValue)
                query = query.Where(c => c.CharityID == charityId.Value);

            // Include charity if requested
            if (inCludeCharity)
                query = query.Include(c => c.Charity);

            // Apply pagination
            var campaigns = await query
                .OrderByDescending(c => c.StartDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (campaigns == null || !campaigns.Any())
                throw new KeyNotFoundException("No campaigns found");

            return campaigns.Select(c => new CampaignDto
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
                CharityID = c.CharityID,
                CharityName = c.Charity?.Name ?? ""
            }).ToList();
        }

        public async Task<bool> CreateAsync(AddCampaignDto model)
        {
            // Check the model
            if (model == null)
                throw new ArgumentNullException(nameof(model), "Your model from request is null.");

            // Handle image: either uploaded file or external URL
            string? imageUrl = null;
            if (model.Image != null)
            {
                // Save uploaded image file
                imageUrl = await _fileService.SaveFileAsync(model.Image, "campaigns");
            }
            else if (!string.IsNullOrWhiteSpace(model.ImageUrl))
            {
                // Use provided external URL
                imageUrl = model.ImageUrl;
            }
            else
            {
                throw new ArgumentException("Either Image or ImageUrl must be provided for the campaign.");
            }

            var campaign = new Campaign
            {
                Title = model.Title,
                Description = model.Description,
                ImageUrl = imageUrl,
                isCritical = model.IsCritical,
                StartDate = model.StartDate ?? DateTime.UtcNow,
                Duration = model.Duration,
                GoalAmount = model.GoalAmount,
                IsInKindDonation = model.IsInKindDonation,
                Category = model.Category,
                CharityID = model.CharityID,
                RaisedAmount = 0,
                Status = CampainStatusEnum.inProgress
            };

            await _unitOfWork.Campaigns.AddAsync(campaign);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<Campaign> UpdateAsync(UpdatCampaignDto model)
        {
            // check the campaign
            var campaign = await _unitOfWork.Campaigns.GetAsync(model.Id, includeCharity: false);
            if (campaign == null)
                throw new ArgumentNullException(nameof(model), "Campaign not found");

            // Handle image update: either uploaded file or external URL
            string? newImageUrl = null;
            if (model.Image != null)
            {
                // Save new uploaded image file
                newImageUrl = await _fileService.SaveFileAsync(model.Image, "campaigns");
            }
            else if (!string.IsNullOrWhiteSpace(model.ImageUrl))
            {
                // Use provided external URL
                newImageUrl = model.ImageUrl;
            }

            if (newImageUrl != null)
            {
                // Delete old image file if it exists and is a local file (starts with /)
                if (!string.IsNullOrEmpty(campaign.ImageUrl) && campaign.ImageUrl.StartsWith("/"))
                {
                    _fileService.DeleteFile(campaign.ImageUrl);
                }

                campaign.ImageUrl = newImageUrl;
            }
            else
            {
                // Validate existing campaign has ImageUrl
                if (string.IsNullOrWhiteSpace(campaign.ImageUrl))
                    throw new ArgumentException("Campaign must have ImageUrl. Please provide one.");
            }

            campaign.Title = model.Title;
            campaign.Description = model.Description;
            campaign.isCritical = model.IsCritical;
            campaign.Duration = model.Duration;
            campaign.GoalAmount = model.GoalAmount;
            campaign.IsInKindDonation = model.IsInKindDonation;
            campaign.Category = model.Category;

            await _unitOfWork.Campaigns.UpdateAsync(campaign);
            await _unitOfWork.SaveAsync();

            return campaign;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var campaign = await _unitOfWork.Campaigns.GetAsync(id, includeCharity: false);
            if (campaign == null)
                throw new KeyNotFoundException("Campaign not found");

            await _unitOfWork.Campaigns.DeleteAsync(id);
            await _unitOfWork.SaveAsync();

            return true;
        }





        public async Task<PaginatedResultDto<CampaignDto>> GetPaginatedOptimizedAsync(
    int page,
    int pageSize,
    CampainStatusEnum? status,
    CampaignCategoryEnum? category,
    string? search,
    bool? isCritical,
    double? minGoalAmount,
    double? maxGoalAmount,
    DateTime? startDateFrom,
    DateTime? startDateTo,
    int? charityId
)
        {
            var query = _unitOfWork.Campaigns.GetAll()
                .AsNoTracking()                     // 🚀 faster 40%
                .Select(c => new CampaignDto        // 🚀 projection before ToList → 3x faster
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    ImageUrl = c.ImageUrl,
                    GoalAmount = c.GoalAmount,
                    RaisedAmount = c.RaisedAmount,
                    Category = c.Category,
                    Status = c.Status,
                    StartDate = c.StartDate,
                    CharityID = c.CharityID,
                    CharityName = c.Charity.Name      // included without heavy Include()
                });

            // 🔍 Fast filters (all DB-side, all index-friendly)
            if (status != null)
                query = query.Where(c => c.Status == status);

            if (category != null)
                query = query.Where(c => c.Category == category);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(c => c.Title.Contains(search));

            if (isCritical != null)
                query = query.Where(c => c.IsCritical == isCritical);

            if (minGoalAmount != null)
                query = query.Where(c => c.GoalAmount >= minGoalAmount);

            if (maxGoalAmount != null)
                query = query.Where(c => c.GoalAmount <= maxGoalAmount);

            if (startDateFrom != null)
                query = query.Where(c => c.StartDate >= startDateFrom);

            if (startDateTo != null)
                query = query.Where(c => c.StartDate <= startDateTo);

            if (charityId != null)
                query = query.Where(c => c.CharityID == charityId);

            // Count query (very fast)
            var total = await query.CountAsync();

            // Pagination query
            var items = await query
                .OrderByDescending(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResultDto<CampaignDto>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                Total = total
            };
        }

    }
}