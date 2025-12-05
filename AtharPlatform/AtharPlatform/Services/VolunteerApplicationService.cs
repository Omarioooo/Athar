using AtharPlatform.DTOs;
using AtharPlatform.Repositories;

namespace AtharPlatform.Services
{
    public class VolunteerApplicationService : IVolunteerApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VolunteerApplicationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<VolunteerApplicationDTO> ApplyAsync(VolunteerApplicationDTO dto)
        {
            if (dto.Age < 11)
                throw new ArgumentException("Age must be greater than 10.");

          
            var charity = await _unitOfWork.Charities.GetByIdAsync(dto.CharityId);
            if (charity == null)
                throw new KeyNotFoundException("Charity not found.");

            
            var slot = await _unitOfWork.CharityVolunteers.GetSlotByCharityIdAsync(dto.CharityId);
            if (slot == null)
            {
                slot = new CharityVolunteer
                {
                    Date = DateTime.UtcNow,
                    IsOpen = true
                };
                await _unitOfWork.CharityVolunteers.AddAsync(slot);
                await _unitOfWork.SaveAsync();
            }



            var application = new VolunteerApplication
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Age = dto.Age,
                PhoneNumber = dto.PhoneNumber,
                Country = dto.Country,
                City = dto.City,
                IsFirstTime = dto.IsFirstTime,
                CharityVolunteerId = slot.CharityVolunteerId
            };

            await _unitOfWork.VolunteerApplications.AddAsync(application);
            await _unitOfWork.SaveAsync();

            return MapToDTO(application);
        }

        public async Task<VolunteerApplicationDTO?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.VolunteerApplications.GetByIdAsync(id);
            return entity != null ? MapToDTO(entity) : null;
        }

        private VolunteerApplicationDTO MapToDTO(VolunteerApplication application) => new VolunteerApplicationDTO
        {
            Id = application.Id,
            FirstName = application.FirstName,
            LastName = application.LastName,
            Age = application.Age,
            PhoneNumber = application.PhoneNumber,
            Country = application.Country,
            City = application.City,
            IsFirstTime = application.IsFirstTime,
          
        };
    }
}

