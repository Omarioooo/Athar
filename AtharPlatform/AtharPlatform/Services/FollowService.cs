using AtharPlatform.Repositories;

namespace AtharPlatform.Services
{
    public class FollowService : IFollowService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;
        public FollowService(IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }

        public async Task<bool> FollowAsync(int donorId, int charityId)
        {
            // check the charity
            var charity = await _unitOfWork.Charities.GetAsync(charityId)
                ?? throw new KeyNotFoundException("Charity not found.");

            // Check if already followed
            var isAlreadyFollowed = await _unitOfWork.Follows.IsFollowedAsync(donorId, charityId);
            if (isAlreadyFollowed)
                throw new InvalidOperationException("You are already following this charity.");

            var follow = new Follow()
            {
                CharityId = charityId,
                DonorId = donorId,
                StartDate = DateTime.UtcNow
            };

            await _unitOfWork.Follows.AddAsync(follow);
            await _unitOfWork.SaveAsync();

            // Notify the charity
            if (_notificationService == null)
                throw new Exception("notification service is null");

            await _notificationService.SendNotificationAsync(donorId, [charityId], Models.Enum.NotificationsTypeEnum.NewFollower);

            return true;
        }

        public async Task<bool> UnFollowAsync(int donorId, int charityId)
        {
            // check the charity
            var charity = await _unitOfWork.Charities.GetAsync(charityId)
                ?? throw new KeyNotFoundException("Charity not found.");

            // check the follow
            var follow = await _unitOfWork.Follows.GetFollowByDonorAndCharityAsync(donorId, charityId)
                ?? throw new InvalidOperationException("You are not following this charity.");

            await _unitOfWork.Follows.DeleteAsync(follow.Id);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<int> GetFollowersCountAsync(int charityId)
        {
            var charity = await _unitOfWork.Charities.GetAsync(charityId);
            if (charity == null) return -1;

            return await _unitOfWork.Follows.GetAll()
                .CountAsync(f => f.CharityId == charityId);
        }

    }
}
