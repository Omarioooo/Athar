
using AtharPlatform.Repositories;

namespace AtharPlatform.Services
{
    public class FollowService : IFollowService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountContextService _accountContextService;


        public FollowService(IUnitOfWork unitOfWork,
            IAccountContextService accountContextService)
        {
            _unitOfWork = unitOfWork;
            _accountContextService = accountContextService;
        }

        public async Task<bool> FollowAsync(int charityId)
        {
            // get the user id
            var userId = _accountContextService.GetCurrentAccountId();

            // check the charity
            var charity = _unitOfWork.Charity.GetAsync(charityId);
            if (charity == null)
                return false;

            // create follow
            Follow follow = new Follow()
            {
                charityID = charity.Id,
                donornID = userId,
                StartDate = DateTime.UtcNow
            };

            // Save the follow
            _unitOfWork.Follows.Add(follow);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<bool> UnFollowAsync(int donorId, int charityId)
        {
            // check the charity
            var charity = await _unitOfWork.Charity.GetAsync(charityId);
            if (charity == null)
                return false;

            // check the follow
            var follow = await _unitOfWork
                .Follows
                .SingleAsync(fl => fl.donornID == donorId && fl.charityID == charityId);

            if (follow == null)
                return false;

            // Remove the follow
            _unitOfWork.Follows.Remove(follow);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> IsFollowedAsync(int donorId, int charityId)
        {
            // Check if there is a follow between donor and charity
            return await _unitOfWork
                .Follows
                .AnyAsync(fl => fl.donornID == donorId && fl.charityID == charityId);
        }

        public Task<bool> FollowAsync(int donorId, int charityId)
        {
            throw new NotImplementedException();
        }
    }
}
