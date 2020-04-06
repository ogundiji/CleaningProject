using System.ComponentModel.DataAnnotations;

namespace CleaningProject.Models
{
    public class RegisterUserModel
    {
        [Required]
        public string Fullname { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(maximumLength: 11, ErrorMessage = "phone number not valid", MinimumLength = 9)]
        public string PhoneNumber { get; set; }
      
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
