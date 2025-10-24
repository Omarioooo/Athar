using System.Linq.Expressions;

namespace AtharPlatform.Repositories
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(Context context) : base(context) { }

        public override async Task<List<Notification>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public override async Task<Notification?> GetAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(n => n.Id == id);
        }

        public override async Task<Notification?> GetAsync(Expression<Func<Notification, bool>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            return await _dbSet.FirstOrDefaultAsync(expression);
        }
        public override async Task<bool> AddAsync(Notification notification)
        {
            if (notification == null)
                return await Task.FromResult(false);

            _dbSet.Add(notification);
            return await Task.FromResult(true);
        }

        public override async Task<bool> Update(Notification notification)
        {
            if (notification == null)
                return await Task.FromResult(false);

            _dbSet.Update(notification);
            return await Task.FromResult(true);
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            var notification = await _dbSet.FirstOrDefaultAsync(e => e.Id == id);

            if (notification == null)
                return await Task.FromResult(false);

            _dbSet.Remove(notification);
            return await Task.FromResult(true);
        }

        public async Task<bool> AddSenderAsync(NotificationSender sender)
        {
            if (sender == null)
                return await Task.FromResult(false);

            _context.Sender.Add(sender);
            return await Task.FromResult(true);
        }

        public async Task<bool> AddReceiversAsync(List<NotificationReceiver> receivers)
        {
            if (receivers == null)
                return await Task.FromResult(false);

            _context.Receivers.AddRange(receivers);
            return await Task.FromResult(true);
        }

        public async Task<List<Notification>> GetNotificationsByUserAsync(int userId)
        {
            return await _context.Notifications
                .Include(n => n.Receivers)
                .Where(n => n.Receivers.Any(r => r.ReceiverId == userId))
                .ToListAsync();
        }

    }
}
