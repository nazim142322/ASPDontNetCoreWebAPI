using System.ComponentModel.DataAnnotations;

namespace NZWalksAPI.Models.DTO
{
    public class AddRegionRequestDTO
    {
        [Required]
        [MinLength(3, ErrorMessage ="Code should be minimum 3 characters")]
        [MaxLength(3, ErrorMessage = "Code should be max 3 characters")]
        public string Code { get; set; } 

        [Required]
        [MaxLength(100, ErrorMessage = "Name should be max 50 characters")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
