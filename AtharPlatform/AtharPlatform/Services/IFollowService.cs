namespace AtharPlatform.Services
{
    public interface IFollowService
    {
        /// <summary>
        /// Asynchronously follows a charity on behalf of a donor.
        /// </summary>
        /// <param name="donorId">The unique identifier of the donor who wishes to follow the charity. Must be a positive integer.</param>
        /// <param name="charityId">The unique identifier of the charity to be followed. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains <see langword="true"/> if the
        /// follow operation was successful; otherwise, <see langword="false"/>.</returns>
        Task<bool> FollowAsync(int donorId, int charityId);

        /// <summary>
        /// Asynchronously unfollows a charity for a specified donor.
        /// </summary>
        /// <param name="donorId">The unique identifier of the donor who wishes to unfollow the charity. Must be a positive integer.</param>
        /// <param name="charityId">The unique identifier of the charity to be unfollowed. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains <see langword="true"/> if the
        /// unfollow operation was successful; otherwise, <see langword="false"/>.</returns>
        Task<bool> UnFollowAsync(int donorId, int charityId);


        Task<int> GetFollowersCountAsync(int charityId);

    }
}