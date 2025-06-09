
using System.ComponentModel.DataAnnotations;

namespace CRUDAppUsingWebAPI.Models
{
    public class Student
    {
        public int id { get; set; }

        [Required]

        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name should contain only letters and spaces")]
        public string studnetName { get; set; }

        [Required]
        public string studentGender { get; set; }

        [Range(1, 100, ErrorMessage = "Age must be between 1 and 100")]
        [Required]
        public int? age { get; set; }

        [Required]
        public int standard { get; set; }

        [Required]
        public string fatherName { get; set; }
    }
}
