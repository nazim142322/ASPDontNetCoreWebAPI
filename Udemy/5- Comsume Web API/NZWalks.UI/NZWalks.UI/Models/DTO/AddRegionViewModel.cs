using System.ComponentModel.DataAnnotations;

namespace NZWalks.UI.Models.DTO
{
    public class AddRegionViewModel
    {
        [Required]
        //[MinLength(3, ErrorMessage ="Code should be minimum 3 characters")]
        //[MaxLength(3, ErrorMessage = "Code should be maximum 3 characters")]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Region Image Url")]
        public string RegionImageUrl { get; set; }
    }
}
