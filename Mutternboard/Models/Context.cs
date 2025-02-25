using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mutterboard.Models
{
    public class Context : IdentityDbContext<ApplicationUser>
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<TaskItem> TaskItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}