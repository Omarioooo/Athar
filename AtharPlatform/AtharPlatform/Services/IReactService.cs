using AtharPlatform.DTOs;

namespace AtharPlatform.Services
{
    public interface IReactService
    {
        Task<bool> React(ReactionDto reactionDto);
        Task<bool> RemoveReact(ReactionDto reactionDto);
    }
}