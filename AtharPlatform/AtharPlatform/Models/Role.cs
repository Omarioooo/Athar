using System.ComponentModel.DataAnnotations;

namespace AtharPlatform.Models
{
    public enum NameRoleEnum
    {
        User,
        Charity
    }
    public class Role
    {
        [Key]
        public int Id { get; set; }


        [Required]
        public NameRoleEnum NameRole { get; set; }



        public List<UserRoles> UserRoles { get; set; }
    }
}
