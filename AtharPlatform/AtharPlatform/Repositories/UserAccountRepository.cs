namespace AtharPlatform.Repositories
{
    public class UserAccountRepository : Repository<UserAccount>, IUserAccountRepository
    {
        public UserAccountRepository(Context context) : base(context)
        {
        }

        public async Task DeleteAsync(UserAccount account)
        {
            _context.Users.Remove(account);
        }
    }
}
