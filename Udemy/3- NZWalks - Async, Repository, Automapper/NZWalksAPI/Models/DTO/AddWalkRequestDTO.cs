using System.ComponentModel.DataAnnotations;

namespace NZWalksAPI.Models.DTO
{
    public class AddWalkRequestDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string Description { get; set; } = string.Empty;
        [Required]
        [Range(0,50)]
        public double LengthInKm { get; set; }
        [Required]
        public string? WalkImageUrl { get; set; }
        [Required]
        public Guid RegionId { get; set; }
        [Required]
        public Guid DifficultyId { get; set; }
    }
}
