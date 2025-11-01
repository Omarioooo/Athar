using AtharPlatform.Models.Enum;
using System.ComponentModel.DataAnnotations;

public class UpdatCampaignDto
{
    [Required]
    public int Id { get; set; }  
    [Required]
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }

    public string Image { get; set; }  

    public bool IsCritical { get; set; }

    public int Duration { get; set; }

    public double GoalAmount { get; set; }

    public bool IsInKindDonation { get; set; }

    public CampaignCategoryEnum Category { get; set; }
}
