using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    //Weak Entity
    public class CharityVolunteer
    {
        [ForeignKey("volunteerForm")]
        public int VolunteerFormId { get; set; }

        public VolunteerForm volunteerForm { get; set; }

        [ForeignKey("Charity")]
        public int CharityId { get; set; }
        public Charity Charity { get; set; }
    }
}
