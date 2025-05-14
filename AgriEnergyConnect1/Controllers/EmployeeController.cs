using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AgriEnergyConnect1.Data;
using AgriEnergyConnect1.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using AgriEnergyConnect1.ViewModels;

namespace AgriEnergyConnect1.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var employeeProfile = await _context.Employees.FirstOrDefaultAsync(e => e.UserId == userId);

            if (employeeProfile == null)
            {
                return RedirectToAction("Create");
            }

            var products = await _context.Products
                .Include(p => p.Farmer)
                .ToListAsync();

            ViewBag.EmployeeName = employeeProfile.Name;
            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            ModelState.Remove("UserId");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError("", "User ID not found. Please try logging in again.");
                return View(employee);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    employee.UserId = userId;
                    _context.Add(employee);
                    await _context.SaveChangesAsync();
                    var addedEmployee = await _context.Employees.FirstOrDefaultAsync(e => e.UserId == userId);
                    if (addedEmployee != null)
                    {
                        return RedirectToAction(nameof(Dashboard));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Profile was not saved properly.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error saving profile: {ex.Message}");
                }
            }
            return View(employee);
        }

        public async Task<IActionResult> ViewFarmers()
        {
            var farmers = await _context.Farmers.ToListAsync();
            return View(farmers);
        }

        public async Task<IActionResult> FarmerProducts(int id, string category, DateTime? startDate, DateTime? endDate)
        {
            var farmer = await _context.Farmers.FindAsync(id);
            var products = _context.Products.Where(p => p.FarmerId == id);

            if (!string.IsNullOrEmpty(category))
                products = products.Where(p => p.Category == category);

            if (startDate.HasValue)
                products = products.Where(p => p.ProductionDate >= startDate.Value);

            if (endDate.HasValue)
                products = products.Where(p => p.ProductionDate <= endDate.Value);

            var viewModel = new FarmerProductsViewModel
            {
                Farmer = farmer,
                Products = await products.ToListAsync()
            };
            return View(viewModel);
        }

        [Authorize(Roles = "Employee")]
        public IActionResult AddFarmer()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Employee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFarmer(Farmer farmer)
        {
            ModelState.Remove("UserId"); 

            if (ModelState.IsValid)
            {
                _context.Farmers.Add(farmer);
                await _context.SaveChangesAsync();
                return RedirectToAction("ViewFarmers");
            }
            return View(farmer);
        }
    }
}