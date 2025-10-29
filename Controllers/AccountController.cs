using Microsoft.AspNetCore.Mvc;
using CMCS.Models;
using CMCS.Data;
using Microsoft.EntityFrameworkCore;

namespace CMCS.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user)
        {
            // Check for hardcoded admin credentials from environment variables
            var adminUsername = Environment.GetEnvironmentVariable("ADMIN_USERNAME");
            var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");

            if (!string.IsNullOrEmpty(adminUsername) && !string.IsNullOrEmpty(adminPassword) &&
                user.Username == adminUsername && user.Password == adminPassword)
            {
                HttpContext.Session.SetString("UserId", "-1"); // Use a special ID for admin
                HttpContext.Session.SetString("UserRole", "AcademicManager");
                HttpContext.Session.SetString("Username", adminUsername);
                TempData["SuccessMessage"] = $"Welcome back, {adminUsername} (Admin)!";
                return RedirectToAction("Dashboard", "AcademicManager");
            }

            if (ModelState.IsValid)
            {
                var foundUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == user.Username && u.Password == user.Password);
                
                if (foundUser != null)
                {
                    if (!foundUser.IsApproved)
                    {
                        ModelState.AddModelError("", "Your account is awaiting approval from an Academic Manager.");
                        return View(user);
                    }
                    // Simple session-based authentication for prototype
                    HttpContext.Session.SetString("UserId", foundUser.UserID.ToString());
                    HttpContext.Session.SetString("UserRole", foundUser.Role ?? "");
                    Console.WriteLine($"DEBUG: UserRole from DB: {foundUser.Role}");
                    Console.WriteLine($"DEBUG: UserRole in Session: {HttpContext.Session.GetString("UserRole")}");
                    HttpContext.Session.SetString("Username", foundUser.Username ?? "");
                    
                    TempData["SuccessMessage"] = $"Welcome back, {foundUser.Username}!";
                    
                    // Redirect based on role
                    var userRole = foundUser.Role?.ToLower() ?? "";
                    return userRole switch
                    {
                        "academicmanager" => RedirectToAction("Dashboard", "AcademicManager"),
                        "programmecoordinator" => RedirectToAction("Dashboard", "ProgrammeCoordinator"),

                        _ => RedirectToAction("Index", "Lecturer")
                    };
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }
            return View(user);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
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

                // Validate and assign role
                var allowedRoles = new[] { "Lecturer" };
                if (string.IsNullOrEmpty(user.Role) || !allowedRoles.Contains(user.Role))
                {
                    ModelState.AddModelError("Role", "Invalid role selected.");
                    return View(user);
                }
                
                Console.WriteLine($"DEBUG: Registering user - Role: {user.Role}, IsApproved: {user.IsApproved}");

                var userToSave = new User
                {
                    Username = user.Username,
                    Password = user.Password,
                    Role = user.Role,
                    IsApproved = true
                };

                _context.Users.Add(userToSave);
                await _context.SaveChangesAsync();

                // If the registered user is a Lecturer, create a corresponding Lecturer entry
                if (userToSave.Role == "Lecturer")
                {
                    var lecturerToSave = new Lecturer
                    {
                        UserID = userToSave.UserID,
                        Name = userToSave.Username, // Or prompt for full name during registration
                        Email = userToSave.Email
                    };
                    _context.Lecturers.Add(lecturerToSave);
                    await _context.SaveChangesAsync();
                }
                
                TempData["SuccessMessage"] = "Registration successful! Please login.";
                return RedirectToAction("Login");
            }
            return View(user);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "You have been logged out successfully.";
            return RedirectToAction("Index", "Home");
        }
    }
}