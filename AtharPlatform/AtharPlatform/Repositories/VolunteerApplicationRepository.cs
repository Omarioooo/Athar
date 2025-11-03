using AtharPlatform.Models;

namespace AtharPlatform.Repositories
{
    public class VolunteerApplicationRepository : Repository<VolunteerApplication>, IVolunteerApplicationRepository
    {
        public VolunteerApplicationRepository(Context context) : base(context) { }

        public override async Task<List<VolunteerApplication>> GetAllAsync()
        {
            return await _dbSet
                 .Include(v => v.CharityVolunteer)
                 .ToListAsync();
        }

        public Task<List<VolunteerApplication>> GetByCampaignAsync(int id)
        {
            throw new NotImplementedException();
        }
    }


}

