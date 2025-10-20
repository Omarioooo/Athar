using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class UserContetReaction
    {
        public DateTime ReactionDate { get; set; }

        [ForeignKey("user")]
        public string UserID { get; set; }
        public User user { get; set; }



        [ForeignKey("Content")]
        public int ContentID { get; set; }
        public Content Content { get; set; }


        [Key,ForeignKey("Reaction")]
        public int ReactionId { get; set; }
        public Reaction Reaction { get; set; }
    }
}
