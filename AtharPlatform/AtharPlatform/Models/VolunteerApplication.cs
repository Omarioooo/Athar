using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{

    public class VolunteerApplication
    {
        [Key]
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
        [ForeignKey(nameof(CharityVolunteer))]
        public int CharityVolunteerId { get; set; }

        public virtual CharityVolunteer CharityVolunteer { get; set; }
    }
}
