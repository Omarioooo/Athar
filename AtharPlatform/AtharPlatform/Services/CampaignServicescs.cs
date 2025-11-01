using AtharPlatform.Dtos;
using AtharPlatform.DTOs;
using AtharPlatform.Models;
using AtharPlatform.Models.Enum;
using AtharPlatform.Repositories;

public class CampaignService : ICampaignService
{
    private readonly ICampaignRepository _repo;
    private readonly IUnitOfWork _unit;

    public CampaignService(ICampaignRepository repo, IUnitOfWork unit)
    {
        _repo = repo;
        _unit = unit;
    }

    public async Task<IEnumerable<CampaignDto>> GetAllAsyncforadmin()
    {
        var campaign = await _repo.GetAllAsync();
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
        var campaign = await _repo.Getallforusers();
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
        var c = await _repo.GetAsync(id);
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
        var data = await _repo.GetByType(type);
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
        var results = await _repo.Search(keyword);
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
        var data = await _repo.GetPaginated(page, pageSize);
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

        await _repo.AddAsync(campaign);
        await _unit.SaveAsync();

        return await GetByIdAsync(campaign.Id);
    }

    public async Task<CampaignDto> UpdateAsync(UpdatCampaignDto model)
    {
        var c = await _repo.GetAsync(model.Id);
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

        _repo.Update(c);
        await _unit.SaveAsync();

        
        return await GetByIdAsync(c.Id);
    }

    public async Task DeleteAsync(int id)
    {
        var c = await _repo.GetAsync(id);
        if (c == null)
            throw new Exception("Campaign not found");

        await _repo.DeleteAsync(id);
        await _unit.SaveAsync();
    }

    public async Task<CampaignDto> GetByIdAsynctousers(int id)
    {
        var c = await _repo.GetAsyncTousers(id);
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

    public  async Task<IEnumerable<CampaignDto>> GetByTypeAsynctousers(CampaignCategoryEnum type)
    {

        var data = await _repo.GetByTypetousers(type);
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
