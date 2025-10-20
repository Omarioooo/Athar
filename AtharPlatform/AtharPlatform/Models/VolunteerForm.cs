using System.ComponentModel.DataAnnotations;

namespace AtharPlatform.Models
{
    
    public class VolunteerForm
    {
        [Key]
        public int  Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public int Age { get; set; }

        [Required,Phone]
        public string PhoneNumber { get; set; }

        [Required,EmailAddress]
        public string Email { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        [Required]
        public string Motivation { get; set; }// Why he want to volunteer (Text)

        public bool FirstTime{ get; set; }

        public List<CharityVolunteer> CharityVolunteer { get; set; }

        
    }
}
