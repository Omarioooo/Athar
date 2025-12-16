namespace AtharPlatform.Models.Enum
{
    /// <summary>
    /// Predefined recommendation count options
    /// </summary>
    public enum RecommendationCount
    {
        /// <summary>
        /// Return top 5 recommendations
        /// </summary>
        Top5 = 5,

        /// <summary>
        /// Return top 10 recommendations
        /// </summary>
        Top10 = 10,

        /// <summary>
        /// Return top 15 recommendations (maximum)
        /// </summary>
        Top15 = 15,

        /// <summary>
        /// Return all available recommendations (no limit)
        /// </summary>
        All = 0
    }
}
