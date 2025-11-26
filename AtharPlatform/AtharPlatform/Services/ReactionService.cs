
using AtharPlatform.DTO;
using AtharPlatform.Models;
using AtharPlatform.Repositories;
using Microsoft.VisualBasic;

namespace AtharPlatform.Services
{
    public class ReactionService : IReactionService
    {
        private readonly IUnitOfWork _unitOfWork;


        public ReactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> React(int donorId, int contentId)
        {
            // check the content
            var content = await _unitOfWork.Contents.GetAsync(contentId)
                ?? throw new KeyNotFoundException("Content not found.");

            // Check if already reacted
            var isAlreadyFollowed = await _unitOfWork.Reactions.IsReactedAsync(donorId, contentId);
            if (isAlreadyFollowed)
                throw new InvalidOperationException("You are already reacted on this content.");

            var react = new Reaction()
            {
                DonorID = donorId,
                ContentID = contentId,
                ReactionDate = DateTime.UtcNow
            };

            await _unitOfWork.Reactions.AddAsync(react);


            var entries = _unitOfWork._context.ChangeTracker.Entries()
    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
    .ToList();

            foreach (var entry in entries)
            {
                Console.WriteLine($"Entity: {entry.Entity.GetType().Name}, State: {entry.State}");
            }


            await _unitOfWork.SaveAsync();

            return true;

        }


        public async Task<bool> RemoveReact(int donorId, int contentId)
        {
            // check the charity
            var charity = await _unitOfWork.Contents.GetAsync(contentId)
                ?? throw new KeyNotFoundException("Content not found.");

            // check the reaction
            var reaction = await  _unitOfWork.Reactions.GetReactionByDonorAndContentAsync(donorId, contentId)
                ?? throw new InvalidOperationException("You are not reacted on this content.");

            await _unitOfWork.Reactions.DeleteAsync(reaction.Id);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
