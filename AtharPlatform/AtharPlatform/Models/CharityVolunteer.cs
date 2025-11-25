using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class CharityVolunteer
    {
        [Key]
        public int CharityVolunteerId { get; set; }
        public bool IsOpen { get; set; } = true;
        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [Required]
        [ForeignKey(nameof(Charity))]
        public int CharityId { get; set; }

        public virtual Charity Charity { get; set; } = null!;

        // Navigation Property
        public virtual List<VolunteerApplication> VolunteerApplications { get; set; } = null!;
    }
}
