using System.ComponentModel.DataAnnotations;

namespace CleaningProject.Models
{
    public class ForgotPasswordModel:GoogleReCaptchaModelBase
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } 
    }
}
