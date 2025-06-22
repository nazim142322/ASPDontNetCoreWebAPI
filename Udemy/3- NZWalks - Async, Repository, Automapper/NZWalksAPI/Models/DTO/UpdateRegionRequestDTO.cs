using System.ComponentModel.DataAnnotations;

namespace NZWalksAPI.Models.DTO
{
    public class UpdateRegionRequestDTO
    {
        [Required]
        [MinLength(3, ErrorMessage ="Code should be min 3 characters")]
        [MaxLength(3, ErrorMessage ="Code should be max 3 characters")]
        public string Code { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Should be 100 charaters long")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
