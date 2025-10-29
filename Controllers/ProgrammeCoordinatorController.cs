using Microsoft.AspNetCore.Mvc;
using CMCS.Models;
using System.Collections.Generic;
using CMCS.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CMCS.Models.ViewModels;

namespace CMCS.Controllers
{
    public class ProgrammeCoordinatorController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public ProgrammeCoordinatorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // This action will handle the request for the dashboard view.
        public IActionResult Dashboard()
        {
            var authCheck = RedirectToLoginIfNotAuthorized("ProgrammeCoordinator");
            if (authCheck != null) return authCheck;

            Console.WriteLine($"DEBUG: ProgrammeCoordinator Dashboard action hit.");
            var pendingClaims = _context.Claims.Where(c => c.Status == ClaimStatus.Submitted).ToList();
            var recentClaims = _context.Claims.OrderByDescending(c => c.DateSubmitted).Take(5).ToList();

            var viewModel = new DashboardViewModel
            {
                PendingClaims = pendingClaims.Count,
                RecentClaims = recentClaims
            };
            Console.WriteLine($"DEBUG: DashboardViewModel - PendingClaims: {viewModel.PendingClaims}");
            Console.WriteLine($"DEBUG: DashboardViewModel - RecentClaims count: {viewModel.RecentClaims.Count}");

            return View(viewModel);
        }

        // Action to handle claims verification and approval
        public IActionResult Approve()
        {
            var authCheck = RedirectToLoginIfNotAuthorized("ProgrammeCoordinator");
            if (authCheck != null) return authCheck;
            
            var claims = _context.Claims.Include(c => c.Lecturer).Where(c => c.Status == ClaimStatus.Submitted).ToList();
            return View(claims);
        }

        public async Task<IActionResult> ApproveClaim(int id)
        {
            var authCheck = RedirectToLoginIfNotAuthorized("ProgrammeCoordinator");
            if (authCheck != null) return authCheck;
            
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null)
            {
                return NotFound();
            }

            claim.Status = ClaimStatus.ApprovedByProgrammeCoordinator;
            _context.Update(claim);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Approve));
        }

        public async Task<IActionResult> RejectClaim(int id)
        {
            var authCheck = RedirectToLoginIfNotAuthorized("ProgrammeCoordinator");
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

        public IActionResult Index()
        {
            var authCheck = RedirectToLoginIfNotAuthorized("ProgrammeCoordinator");
            if (authCheck != null) return authCheck;
            
            return View();
        }

        public IActionResult Create()
        {
            var authCheck = RedirectToLoginIfNotAuthorized("ProgrammeCoordinator");
            if (authCheck != null) return authCheck;
            
            return View();
        }

    }
}
