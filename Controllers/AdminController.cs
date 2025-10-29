using Microsoft.AspNetCore.Mvc;
using CMCS.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using CMCS.Models;

namespace CMCS.Controllers
{
    [Authorize(Roles = "AcademicManager")]
    public class AdminController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ManageUsers()
        {
            var authCheck = RedirectToLoginIfNotAuthorized("AcademicManager");
            if (authCheck != null) return authCheck;

            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        // GET: Admin/AddUser
        public IActionResult AddUser()
        {
            var authCheck = RedirectToLoginIfNotAuthorized("AcademicManager");
            if (authCheck != null) return authCheck;

            return View();
        }

        // POST: Admin/AddUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(User user)
        {
            var authCheck = RedirectToLoginIfNotAuthorized("AcademicManager");
            if (authCheck != null) return authCheck;

            if (ModelState.IsValid)
            {
                // Check if username already exists
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == user.Username);
                
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "Username already exists.");
                    return View(user);
                }

                // For admin added users, they are approved by default
                user.IsApproved = true;
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "User added successfully!";
                return RedirectToAction(nameof(ManageUsers));
            }
            return View(user);
        }

        // POST: Admin/DeleteUser/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var authCheck = RedirectToLoginIfNotAuthorized("AcademicManager");
            if (authCheck != null) return authCheck;

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "User deleted successfully!";
            return RedirectToAction(nameof(ManageUsers));
        }
    }
}