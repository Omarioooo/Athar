namespace AtharPlatform.Repositories
{
    public interface IFollowRepository : IRepository<Follow>
    {
        /// <summary>
        /// Determines whether a specified donor is following a specified charity asynchronously.
        /// </summary>
        /// <param name="donorId">The unique identifier of the donor.</param>
        /// <param name="charityId">The unique identifier of the charity.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains  true if the donor is following
        /// the charity; otherwise, false.</returns>
        Task<bool> IsFollowedAsync(int donorId, int charityId);

        /// <summary>
        /// Asynchronously retrieves the follow relationship between a donor and a charity.
        /// </summary>
        /// <param name="donorId">The unique identifier of the donor. Must be a positive integer.</param>
        /// <param name="charityId">The unique identifier of the charity. Must be a positive integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="Follow"/> object
        /// representing the follow relationship, or <see langword="null"/> if no such relationship exists.</returns>
        Task<Follow> GetFollowAsync(int donorId, int charityId);


        /// <summary>
        /// Asynchronously retrieves a list of charity IDs that the specified donor is following.
        /// </summary>
        /// <param name="donorId">The unique identifier of the donor whose followed charities are to be retrieved. Must be a positive integer.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of integers, each
        /// representing the ID of a charity followed by the donor. The list will be empty if the donor is not following
        /// any charities.</returns>
        Task<List<int>> GetAllFollowedCharitiesByDonorAsync(int donorId);

        /// <summary>
        /// Asynchronously retrieves a list of donors IDs that the specified donor is followr.
        /// </summary>
        /// <param name="charityId"></param>
        /// <returns>A task representing the asynchronous operation. The task result contains a list of integers, each
        /// representing the ID of a donor follower follower for the charity. The list will be empty if the charity has no followers
        /// any charities.</returns>
        Task<List<int>> GetAllFollowersForCharityAsync(int charityId);
    }
}