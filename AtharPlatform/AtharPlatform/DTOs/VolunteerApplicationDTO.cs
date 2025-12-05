
using System.ComponentModel.DataAnnotations;

namespace AtharPlatform.DTOs
{
    public class VolunteerApplicationDTO
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public int Age { get; set; }

        [Required, Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }

        public bool IsFirstTime { get; set; } = true;

        [Required]
        //public int CharityVolunteerId { get; set; }
        public int CharityId { get; set; }

        public string Type = "Volunteer";

        public DateTime Date { get; set; }

   

    }
}
