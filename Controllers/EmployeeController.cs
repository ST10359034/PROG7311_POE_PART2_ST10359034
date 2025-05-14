using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AgriEnergyConnect1.Data;
using AgriEnergyConnect1.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using AgriEnergyConnect1.ViewModels;

namespace AgriEnergyConnect1.Controllers
{
    // This controller manages all actions related to Employee users
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        // The database context for accessing data
        private readonly ApplicationDbContext _context;

        // Constructor injects the database context
        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Displays the employee dashboard with a list of all products and employee's name
        public async Task<IActionResult> Dashboard()
        {
            // Get the current logged-in user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Find the employee profile associated with this user
            var employeeProfile = await _context.Employees.FirstOrDefaultAsync(e => e.UserId == userId);

            // If the employee profile doesn't exist, redirect to profile creation
            if (employeeProfile == null)
            {
                return RedirectToAction("Create");
            }

            // Get all products, including related farmer information
            var products = await _context.Products
                .Include(p => p.Farmer)
                .ToListAsync();

            // Pass the employee's name to the view
            ViewBag.EmployeeName = employeeProfile.Name;
            return View(products);
        }

        // Displays the form to create a new employee profile
        public IActionResult Create()
        {
            return View();
        }

        // Handles the submission of the employee profile creation form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            // Remove UserId from validation since it will be set here
            ModelState.Remove("UserId");
            // Get the current user's ID
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
                    // Set the UserId and save the new employee profile
                    employee.UserId = userId;
                    _context.Add(employee);
                    await _context.SaveChangesAsync();
                    // Confirm the profile was saved and redirect to dashboard
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
                    // Handle any errors that occur during save
                    ModelState.AddModelError("", $"Error saving profile: {ex.Message}");
                }
            }
            // If model state is invalid or save fails, return the view with errors
            return View(employee);
        }

        // Displays a list of all farmers in the system
        public async Task<IActionResult> ViewFarmers()
        {
            var farmers = await _context.Farmers.ToListAsync();
            return View(farmers);
        }

        // Shows products for a specific farmer, with optional filtering by category and date range
        public async Task<IActionResult> FarmerProducts(int id, string category, DateTime? startDate, DateTime? endDate)
        {
            // Find the farmer by ID
            var farmer = await _context.Farmers.FindAsync(id);
            // Start with all products for this farmer
            var products = _context.Products.Where(p => p.FarmerId == id);

            // Filter by category if provided
            if (!string.IsNullOrEmpty(category))
                products = products.Where(p => p.Category == category);

            // Filter by production date range if provided
            if (startDate.HasValue)
                products = products.Where(p => p.ProductionDate >= startDate.Value);

            if (endDate.HasValue)
                products = products.Where(p => p.ProductionDate <= endDate.Value);

            // Prepare the view model with farmer and filtered products
            var viewModel = new FarmerProductsViewModel
            {
                Farmer = farmer,
                Products = await products.ToListAsync()
            };
            return View(viewModel);
        }

        // Displays the form to add a new farmer (only for employees)
        [Authorize(Roles = "Employee")]
        public IActionResult AddFarmer()
        {
            return View();
        }

        // Handles the submission of the add farmer form
        [HttpPost]
        [Authorize(Roles = "Employee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFarmer(Farmer farmer)
        {
            // Remove UserId from validation since it's not set here
            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
                // Add the new farmer to the database
                _context.Farmers.Add(farmer);
                await _context.SaveChangesAsync();
                // Redirect to the list of farmers after successful addition
                return RedirectToAction("ViewFarmers");
            }
            // If model state is invalid, return the view with errors
            return View(farmer);
        }
    }
}