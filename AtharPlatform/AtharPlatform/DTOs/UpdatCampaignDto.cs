using AtharPlatform.Models.Enum;
using AtharPlatform.Validators;
using System.ComponentModel.DataAnnotations;

public class UpdatCampaignDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }

    /// <summary>
    /// Binary image file (for manual uploads). Either Image or ImageUrl must be provided, but not both.
    /// </summary>
    public IFormFile? Image { get; set; }

    /// <summary>
    /// External image URL (for campaigns with hosted images). Either Image or ImageUrl must be provided, but not both.
    /// </summary>
    [ValidImageUrl]
    public string? ImageUrl { get; set; }

    public bool IsCritical { get; set; }

    public int Duration { get; set; }

    public double GoalAmount { get; set; }

    public bool IsInKindDonation { get; set; }

    public CampaignCategoryEnum Category { get; set; }
}
