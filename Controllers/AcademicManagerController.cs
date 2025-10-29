using Microsoft.AspNetCore.Mvc;
using CMCS.Models;
using System.Collections.Generic;
using CMCS.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CMCS.Models.ViewModels;

namespace CMCS.Controllers
{
    public class AcademicManagerController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public AcademicManagerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            var authCheck = RedirectToLoginIfNotAuthorized("AcademicManager");
            if (authCheck != null) return authCheck;

            var pendingClaims = _context.Claims.Where(c => c.Status == ClaimStatus.ApprovedByProgrammeCoordinator).ToList();
            var recentClaims = _context.Claims.OrderByDescending(c => c.DateSubmitted).Take(5).ToList();

            var viewModel = new DashboardViewModel
            {
                PendingClaims = pendingClaims.Count,
                RecentClaims = recentClaims
            };

            return View(viewModel);
        }

        // Action to handle claims verification and approval
        public IActionResult Approve()
        {
            var authCheck = RedirectToLoginIfNotAuthorized("AcademicManager");
            if (authCheck != null) return authCheck;
            
            var claims = _context.Claims.Include(c => c.Lecturer).Where(c => c.Status == ClaimStatus.ApprovedByProgrammeCoordinator).ToList();
            return View(claims);
        }

        public async Task<IActionResult> ApproveClaim(int id)
        {
            var authCheck = RedirectToLoginIfNotAuthorized("AcademicManager");
            if (authCheck != null) return authCheck;
            
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null)
            {
                return NotFound();
            }

            claim.Status = ClaimStatus.ApprovedByAcademicManager;
            _context.Update(claim);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Approve));
        }

        public async Task<IActionResult> RejectClaim(int id)
        {
            var authCheck = RedirectToLoginIfNotAuthorized("AcademicManager");
            if (authCheck != null) return authCheck;
            
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null)
            {
                return NotFound();
            }

            claim.Status = ClaimStatus.Rejected;
            _context.Update(claim);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Approve));
        }

        public IActionResult ApproveProgrammeCoordinators()
        {
            var authCheck = RedirectToLoginIfNotAuthorized("AcademicManager");
            if (authCheck != null) return authCheck;

            var unapprovedProgrammeCoordinators = _context.Users
                .Where(u => u.Role == "ProgrammeCoordinator" && !u.IsApproved)
                .ToList();

            return View(unapprovedProgrammeCoordinators);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveUser(int id)
        {
            var authCheck = RedirectToLoginIfNotAuthorized("AcademicManager");
            if (authCheck != null) return authCheck;

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.IsApproved = true;
            _context.Update(user);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"User {user.Username} has been approved.";
            return RedirectToAction(nameof(ApproveProgrammeCoordinators));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectUser(int id, string rejectionReason)
        {
            var authCheck = RedirectToLoginIfNotAuthorized("AcademicManager");
            if (authCheck != null) return authCheck;

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.IsApproved = false;
            user.RejectionReason = rejectionReason;
            _context.Update(user);
            await _context.SaveChangesAsync();

            TempData["ErrorMessage"] = $"User {user.Username} has been rejected. Reason: {rejectionReason}";
            return RedirectToAction(nameof(ApproveProgrammeCoordinators));
        }

        public IActionResult Create()
        {
            var authCheck = RedirectToLoginIfNotAuthorized("AcademicManager");
            if (authCheck != null) return authCheck;
            
            return View();
        }

    }
}
