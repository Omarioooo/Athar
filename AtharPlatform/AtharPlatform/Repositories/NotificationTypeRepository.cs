namespace AtharPlatform.Repositories
{
    public class NotificationTypeRepository : Repository<NotificationType>, INotificationTypeRepository
    {
        public NotificationTypeRepository(Context context) : base(context) { }

        public async Task<NotificationType?> GetNotificationTypeByIdAsync(int id)
            => await _dbSet.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<NotificationType?> GetNotificationTypeByNameAsync(string name)
            => await _dbSet.FirstOrDefaultAsync(x => x.Name == name);
    }
}
