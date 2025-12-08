namespace AtharPlatform.Repositories
{
    public class UserAccountRepository : Repository<UserAccount>, IUserAccountRepository
    {
        public UserAccountRepository(Context context) : base(context)
        {
        }
    }
}
