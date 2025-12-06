namespace AtharPlatform.Repositories
{
    public class CharityVolunteerRepository : ICharityVolunteerRepository
    {
        private readonly Context _context;

        public CharityVolunteerRepository(Context context)
        {
            _context = context;
        }

        public async Task<CharityVolunteer?> GetByIdAsync(int id)
        {
            return await _context.CharityVolunteers
                .Include(c => c.VolunteerApplications)
                .FirstOrDefaultAsync(c => c.CharityVolunteerId == id);
        }

        public async Task<IEnumerable<CharityVolunteer>> GetByCharityIdAsync(int charityId)
        {
            return await _context.CharityVolunteers
                .Where(v => v.CharityId == charityId)
                 .Include(c => c.Charity)
                 .Include(v => v.VolunteerApplications)
                .ToListAsync();
        }
        

        public async Task AddAsync(CharityVolunteer entity)
        {
            await _context.CharityVolunteers.AddAsync(entity);
        }

        public async Task<IEnumerable<CharityVolunteer>> GetAllAsync()
        {
            return await _context.CharityVolunteers.ToListAsync();
        }

        public async Task<bool> ExistsAsync(int charityId)
        {
            return await _context.CharityVolunteers
                .AnyAsync(c => c.CharityId == charityId);
        }

        public async Task<CharityVolunteer?> GetSlotByCharityIdAsync(int charityId)
        {
            return await _context.CharityVolunteers
                                 .FirstOrDefaultAsync(c => c.CharityId == charityId);
        }
    }
}
