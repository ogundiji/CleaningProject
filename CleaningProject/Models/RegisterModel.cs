using System.ComponentModel.DataAnnotations;

namespace CleaningProject.Models
{
    public class RegisterModel : GoogleReCaptchaModelBase
    {
        [Required]
        public string Fullname { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(11)]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

    }
    public class RegisterStaffModel:GoogleReCaptchaModelBase
    {
        [Required]
        public string Fullname { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(maximumLength: 11, ErrorMessage = "phone number not valid", MinimumLength = 9)]
        public string PhoneNumber { get; set; }
        public byte[] UserImage { get; set; }
        [Required]
        public string UserRoles { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

    }

    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
