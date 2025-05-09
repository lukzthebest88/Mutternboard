using System.ComponentModel.DataAnnotations;

namespace Mutternboard.Models
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Aktuelles Passwort")]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Neues Passwort")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwörter stimmen nicht überein.")]
        [Display(Name = "Neues Passwort bestätigen")]
        public string ConfirmPassword { get; set; }
    }
}
