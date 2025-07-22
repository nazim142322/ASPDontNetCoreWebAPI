using System.ComponentModel.DataAnnotations;

namespace NZWalks.UI.Models.DTO
{
    public class AddRegionViewModel
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Region Image Url")]
        public string RegionImageUrl { get; set; }
    }
}
