using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mutternboard.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base()
        {
            ConcurrencyStamp = Guid.NewGuid().ToString(); // Zufälligen Wert setzen
        }

        public ApplicationRole(string roleName) : base(roleName)
        {
            ConcurrencyStamp = Guid.NewGuid().ToString(); // Zufälligen Wert setzen
        }

    }
}
