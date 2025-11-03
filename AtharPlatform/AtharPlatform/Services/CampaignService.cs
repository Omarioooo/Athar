using AtharPlatform.Dtos;
using AtharPlatform.DTOs;
using AtharPlatform.Models;
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

        public async Task<List<CampaignDto>> GetAllAsync(bool inProgress = true, bool includeCharity = true)
        {
            List<Campaign> campaigns;

            if (inProgress)
                campaigns = await _unitOfWork.Campaigns.GetAllInProgressAsync(includeCharity);
            else
                campaigns = await _unitOfWork.Campaigns.GetAllAsync(includeCharity);

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
                CharityName = c.Charity?.Name ?? ""
            }).ToList(); ;
        }

        public async Task<CampaignDto> GetAsync(int id, bool inCludeCharity = true)
        {
            var campaign = await _unitOfWork.Campaigns.GetAsync(id, inCludeCharity);
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
                CharityName = c.Charity?.Name ?? ""
            }).ToList();
        }

        public async Task<List<CampaignDto>> GetPaginatedAsync(int page, int pageSize, bool inCludeCharity = true)
        {
            var campaigns = await _unitOfWork.Campaigns.GetPaginatedAsync(page, pageSize, inCludeCharity);

            if (campaigns == null || !campaigns.Any())
                throw new Exception("No campaigns found");

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
                CharityName = c.Charity?.Name ?? ""
            }).ToList();
        }

        public async Task<bool> CreateAsync(AddCampaignDto model)
        {
            // Check the model
            if (model == null)
                throw new ArgumentNullException("Your model from request is null.");

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

        public async Task<bool> UpdateAsync(UpdatCampaignDto model)
        {
            // check the campaign
            var campaign = await _unitOfWork.Campaigns.GetAsync(model.Id, includeCharity: false);
            if (campaign == null)
                throw new Exception("Campaign not found");

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


            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var campaign = await _unitOfWork.Campaigns.GetAsync(id, includeCharity: false);
            if (campaign == null)
                throw new Exception("Campaign not found");

            await _unitOfWork.Campaigns.DeleteAsync(id);
            await _unitOfWork.SaveAsync();

            return true;
        }

    }
}
