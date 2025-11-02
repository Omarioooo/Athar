
using AtharPlatform.DTO;
using AtharPlatform.Repositories;

namespace AtharPlatform.Services
{
    public class ReactService : IReactService
    {
        private readonly IUnitOfWork _unitOfWork;


        public ReactService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> React(ReactionDto model)
        {
            // check the donor
            var donor = _unitOfWork.Donors.GetAsync(model.donorId)
                ?? throw new Exception($"Donor with id {model.donorId} not found");

            // check the content
            var content = _unitOfWork.Contents.GetAsync(model.contentId)
                ?? throw new Exception($"Content with id {model.contentId} not found");

            // check the reaction
            var reaction = _unitOfWork.Reactions.GetByDonorAndContent(model);

            if (reaction != null)
                return true;

            var react = new Reaction()
            {
                DonorID = model.donorId,
                ContentID = model.contentId
            };

            await _unitOfWork.Reactions.AddAsync(react);

            return true;

        }

        public async Task<bool> RemoveReact(ReactionDto model)
        {
            return true;
        }
    }
}
