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

        // Get all applications for a specific volunteer opportunity
        public async Task<List<VolunteerApplication>> GetByCampaignAsync(int charityVolunteerId)
        {
            return await _dbSet
                .Where(v => v.CharityVolunteerId == charityVolunteerId)
                .Include(v => v.CharityVolunteer)
                .ToListAsync();
        }
    }


}

