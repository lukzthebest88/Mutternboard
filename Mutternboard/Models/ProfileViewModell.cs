using System.ComponentModel.DataAnnotations;

namespace Mutternboard.Models
{
    public class ProfileViewModel
    {
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
