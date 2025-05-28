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
    public class TaskController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TaskController(Context context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            List<TaskItem> tasks;

            if (User.IsInRole("Admin"))
            {
                tasks = await _context.TaskItems
                    .Include(t => t.User)
                    .ToListAsync();
            }
            else
            {
                tasks = await _context.TaskItems
                    .Where(t => t.UserId == userId)
                    .ToListAsync();
            }

            return View(tasks);
        }

        public IActionResult TaskPlanning()
        {
            var userId = _userManager.GetUserId(User);
            var model = _context.TaskItems
                                .Include(t => t.User)
                                .Where(t => t.UserId == userId)
                                .OrderByDescending(t => t.Priority)
                                .ToList();
            return View(model);  // lädt Views/Task/TaskPlanning.cshtml
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Users = await _context.Users.ToListAsync();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(TaskItem task)
        {
            if (string.IsNullOrEmpty(task.UserId))
            {
                ModelState.AddModelError("UserId", "Bitte einen Benutzer auswählen.");
                ViewBag.Users = await _context.Users.ToListAsync();
                return View(task);
            }

            // User-Objekt aus der Datenbank laden und dem Task zuweisen
            task.User = await _context.Users.FindAsync(task.UserId);

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState ist NICHT valid!");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Fehler: {error.ErrorMessage}");
                }

                ViewBag.Users = await _context.Users.ToListAsync();
                return View(task);
            }

            Console.WriteLine("ModelState ist valid, Aufgabe wird gespeichert...");
            _context.Add(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }
            return View(taskItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,StartDate,EndDate,Priority,UserId")] TaskItem taskItem)
        {
            if (id != taskItem.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(taskItem);
            }

            try
            {
                var existingTask = await _context.TaskItems.FindAsync(id);
                if (existingTask == null)
                {
                    return NotFound();
                }

                // Benutzer-ID nicht überschreiben, falls sie schon existiert
                taskItem.UserId = existingTask.UserId;

                _context.Entry(existingTask).CurrentValues.SetValues(taskItem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.TaskItems.Any(t => t.Id == taskItem.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }

            _context.TaskItems.Remove(taskItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index"); // Zurück zur Admin-Panel-Übersicht
        }

        [HttpPost]
        public async Task<IActionResult> CompleteTask(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            task.IsCompleted = true;
            _context.TaskItems.Update(task);
            await _context.SaveChangesAsync();

            return RedirectToAction("TaskPlanning", "Task");
        }

        [HttpPost]
        public async Task<IActionResult> ReopenTask(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            task.IsCompleted = false;
            _context.TaskItems.Update(task);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


    }
}