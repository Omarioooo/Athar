namespace AtharPlatform.Services
{
    public interface IFollowService
    {
        Task<bool> FollowAsync(int donorId, int charityId);
        Task<bool> UnFollowAsync(int donorId, int charityId);
        Task<bool> IsFollowedAsync(int donorId, int charityId);
    }
}