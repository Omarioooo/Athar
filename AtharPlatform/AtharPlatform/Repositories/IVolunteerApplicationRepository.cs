namespace AtharPlatform.Repositories
{
    public interface IVolunteerApplicationRepository : IRepository<VolunteerApplication>
    {
        Task<List<VolunteerApplication>> GetByCampaignAsync(int id);
    }
}
