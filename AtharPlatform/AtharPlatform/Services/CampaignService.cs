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

        public CampaignService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                Image = c.Image,
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
                Image = campaign.Image,
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
                Image = c.Image,
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
                Image = c.Image,
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
                Image = c.Image,
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

            // Dealing with files
            using MemoryStream stream = new MemoryStream();
            if (model.Image != null)
            {
                await model.Image.CopyToAsync(stream);
            }

            var campaign = new Campaign
            {
                Title = model.Title,
                Description = model.Description,
                Image = stream.ToArray(),
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

            // deal with files
            using MemoryStream stream = new MemoryStream();
            if (model.Image != null)
            {
                await model.Image.CopyToAsync(stream);
            }

            campaign.Title = model.Title;
            campaign.Description = model.Description;
            campaign.Image = stream.ToArray();
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
    }
}