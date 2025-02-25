using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mutterboard.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Mutterboard.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(Context context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var tasks = await _context.TaskItems.Where(t => t.UserId == userId).ToListAsync();
            return View(tasks);
        }
    }
}
