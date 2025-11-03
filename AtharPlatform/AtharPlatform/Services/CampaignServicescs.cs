using AtharPlatform.Dtos;
using AtharPlatform.DTOs;
using AtharPlatform.Models;
using AtharPlatform.Models.Enum;
using AtharPlatform.Repositories;

public class CampaignService : ICampaignService
{
    private readonly ICampaignRepository _campaignRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CampaignService(ICampaignRepository campaignRepository, IUnitOfWork unitOfWork)
    {
        _campaignRepository = campaignRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CampaignDto>> GetAllAsyncforadmin()
    {
        var campaign = await _campaignRepository.GetAllAsync();
        return campaign.Select(c => new CampaignDto
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            Image = c.ImageUrl,
            GoalAmount = c.GoalAmount,
            RaisedAmount = c.RaisedAmount,
            StartDate = c.StartDate,
            EndDate = c.EndDate,
            Category = c.Category,
            CharityName = c.Charity?.Name
        });
    }
    public async Task<IEnumerable<CampaignDto>> GetAllAsyncforusers()
    {
        var campaign = await _campaignRepository.GetAllInProgressWithCharitiesAsync();
        return campaign.Select(c => new CampaignDto
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            Image = c.ImageUrl,
            GoalAmount = c.GoalAmount,
            RaisedAmount = c.RaisedAmount,
            StartDate = c.StartDate,
            EndDate = c.EndDate,
            Category = c.Category,
            CharityName = c.Charity?.Name
        });
    }

    public async Task<CampaignDto> GetByIdAsync(int id)
    {
        var c = await _campaignRepository.GetAsync(id);
        if (c == null)
            throw new Exception("Campaign not found");

        return new CampaignDto
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            Image = c.ImageUrl,
            GoalAmount = c.GoalAmount,
            RaisedAmount = c.RaisedAmount,
            StartDate = c.StartDate,
            EndDate = c.EndDate,
            Category = c.Category,
            CharityName = c.Charity?.Name
        };
    }

    public async Task<IEnumerable<CampaignDto>> GetByTypeAsync(CampaignCategoryEnum type)
    {
        var data = await _campaignRepository.GetAllByTypeAsync(type);
        if (data == null)
            throw new Exception("Campaign not found");

        return data.Select(c => new CampaignDto
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            Image = c.ImageUrl,
            GoalAmount = c.GoalAmount,
            RaisedAmount = c.RaisedAmount,
            StartDate = c.StartDate,
            EndDate = c.EndDate,
            Category = c.Category,
            CharityName = c.Charity?.Name
        });
    }

    public Task<IEnumerable<object>> GetAllTypesAsync()
    {
        var types = Enum.GetValues(typeof(CampaignCategoryEnum))
            .Cast<CampaignCategoryEnum>()
            .Select(t => (object)new
            {
                Value = (int)t,
                Name = t.ToString()
            });

        return Task.FromResult(types);
    }

    public async Task<IEnumerable<CampaignDto>> SearchAsync(string keyword)
    {
        var results = await _campaignRepository.Search(keyword);
        return results.Select(c => new CampaignDto
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            Image = c.ImageUrl,
            GoalAmount = c.GoalAmount,
            RaisedAmount = c.RaisedAmount,
            StartDate = c.StartDate,
            EndDate = c.EndDate,
            Category = c.Category,
            CharityName = c.Charity?.Name
        });
    }

    public async Task<IEnumerable<CampaignDto>> GetPaginatedAsync(int page, int pageSize)
    {
        var data = await _campaignRepository.GetPaginatedAsync(page, pageSize);
        if (data == null || !data.Any())
            throw new Exception("No campaigns found");

        return data.Select(c => new CampaignDto
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            Image = c.ImageUrl,
            GoalAmount = c.GoalAmount,
            RaisedAmount = c.RaisedAmount,
            StartDate = c.StartDate,
            EndDate = c.EndDate,
            Category = c.Category,
            CharityName = c.Charity?.Name
        });
    }

    public async Task<CampaignDto> CreateAsync(AddCampaignDto model)
    {
        var campaign = new Campaign
        {
            Title = model.Title,
            Description = model.Description,
            ImageUrl = model.Image,
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

        await _campaignRepository.AddAsync(campaign);
        await _unitOfWork.SaveAsync();

        return await GetByIdAsync(campaign.Id);
    }

    public async Task<CampaignDto> UpdateAsync(UpdatCampaignDto model)
    {
        var c = await _campaignRepository.GetAsync(model.Id);
        if (c == null)
            throw new Exception("Campaign not found");

        c.Title = model.Title;
        c.Description = model.Description;
        c.ImageUrl = model.Image;
        c.isCritical = model.IsCritical;
        c.Duration = model.Duration;
        c.GoalAmount = model.GoalAmount;
        c.IsInKindDonation = model.IsInKindDonation;
        c.Category = model.Category;

        await _campaignRepository.UpdateAsync(c);
        await _unitOfWork.SaveAsync();


        return await GetByIdAsync(c.Id);
    }

    public async Task DeleteAsync(int id)
    {
        var c = await _campaignRepository.GetAsync(id);
        if (c == null)
            throw new Exception("Campaign not found");

        await _campaignRepository.DeleteAsync(id);
        await _unitOfWork.SaveAsync();
    }

    public async Task<CampaignDto> GetByIdAsynctousers(int id)
    {
        var c = await _campaignRepository.GetWithCharityAsync(id);
        if (c == null)
            throw new Exception("Campaign not found");

        return new CampaignDto
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            Image = c.ImageUrl,
            GoalAmount = c.GoalAmount,
            RaisedAmount = c.RaisedAmount,
            StartDate = c.StartDate,
            EndDate = c.EndDate,
            Category = c.Category,
            CharityName = c.Charity?.Name
        };
    }

    public async Task<IEnumerable<CampaignDto>> GetByTypeAsynctousers(CampaignCategoryEnum type)
    {

        var data = await _campaignRepository.GetAllInProgressByTypeAsync(type);
        if (data == null)
            throw new Exception("Campaign not found");

        return data.Select(c => new CampaignDto
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            Image = c.ImageUrl,
            GoalAmount = c.GoalAmount,
            RaisedAmount = c.RaisedAmount,
            StartDate = c.StartDate,
            EndDate = c.EndDate,
            Category = c.Category,
            CharityName = c.Charity?.Name
        });
    }
}
