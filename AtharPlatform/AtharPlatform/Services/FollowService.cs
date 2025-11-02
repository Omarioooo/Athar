using AtharPlatform.Repositories;

namespace AtharPlatform.Services
{
    public class FollowService : IFollowService
    {
        private readonly IUnitOfWork _unitOfWork;
        public FollowService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                charityID = charityId,
                donornID = donorId,
                StartDate = DateTime.UtcNow
            };

            await _unitOfWork.Follows.AddAsync(follow);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<bool> UnFollowAsync(int donorId, int charityId)
        {
            // check the charity
            var charity = await _unitOfWork.Charities.GetAsync(charityId)
                ?? throw new KeyNotFoundException("Charity not found.");

            // check the follow
            var follow = await _unitOfWork.Follows.GetFollowAsync(donorId, charityId)
                ?? throw new InvalidOperationException("You are not following this charity.");

            await _unitOfWork.Follows.DeleteAsync(follow.Id);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
