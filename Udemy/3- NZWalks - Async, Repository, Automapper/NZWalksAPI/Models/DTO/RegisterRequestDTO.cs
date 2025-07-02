using System.ComponentModel.DataAnnotations;

namespace NZWalksAPI.Models.DTO
{
    public class RegisterRequestDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string? Roles { get; set; }    


    }
}
