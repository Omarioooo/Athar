using AtharPlatform.DTO;

namespace AtharPlatform.Services
{
    public interface IReactService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="donorId"></param>
        /// <param name="contentId"></param>
        /// <returns></returns>
        Task<bool> React(int donorId, int contentId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="donorId"></param>
        /// <param name="contentId"></param>
        /// <returns></returns>
        Task<bool> RemoveReact(int donorId, int contentId);
    }
}