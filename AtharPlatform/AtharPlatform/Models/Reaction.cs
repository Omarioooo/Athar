using System.ComponentModel.DataAnnotations;

namespace AtharPlatform.Models
{
    public enum ReactionTypeEnum
    { 
        Like,
        DisLike
    }
    public class Reaction

    {
        [Key]
        public int Id { get; set; }

         
        [MaxLength(50)]
        public ReactionTypeEnum ReactionType { get; set; }

        public bool IsDeleted { get; set; }


        public List<UserContetReaction> UserContetReaction { get; set; }

    }
}
