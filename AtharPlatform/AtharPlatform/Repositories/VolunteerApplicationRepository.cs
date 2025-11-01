namespace AtharPlatform.Repositories
{
    public class VolunteerApplicationRepository : Repository<VolunteerApplication>, IVolunteerApplicationRepository
    {
        public VolunteerApplicationRepository(Context context) : base(context) { }

        public Task<List<VolunteerApplication>> GetByCampaignAsync(int charityVolunteerId)
        {
            return _context.VolunteerForm
                .Where(v => v.CharityVolunteerId == charityVolunteerId)
                .Include(v => v.CharityVolunteer)
                .ToListAsync();
        }
    }
}
