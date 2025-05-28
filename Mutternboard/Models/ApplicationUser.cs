using Microsoft.AspNetCore.Identity;

namespace Mutterboard.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty; // Default-Wert setzen

    }
}
