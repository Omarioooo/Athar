using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class UserRoles
    {

        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role Role { get; set; }


        [ForeignKey("UserAccount")]
        public string AccountId { get; set; }
        public UserAccount UserAccount { get; set; }
    }
}
