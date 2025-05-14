using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AgriEnergyConnect1.Data;
using AgriEnergyConnect1.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AgriEnergyConnect1.Controllers
{
    // This controller manages all actions related to Farmer users
    [Authorize(Roles = "Farmer")]
    public class FarmerController : Controller
    {
        // The database context for accessing data
        private readonly ApplicationDbContext _context;

        // Constructor injects the database context
        public FarmerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Displays the farmer dashboard with a list of their products and farmer's name
        public async Task<IActionResult> Dashboard()
        {
            // Get the current logged-in user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                System.Diagnostics.Debug.WriteLine("[Dashboard] User ID is null or empty. Redirecting to Login.");
                return RedirectToAction("Login", "Account");
            }
            // Find the farmer profile associated with this user
            var farmerProfile = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == userId);
            if (farmerProfile == null)
            {
                System.Diagnostics.Debug.WriteLine($"[Dashboard] No farmer profile found for user ID: {userId}. Redirecting to Create.");
                return RedirectToAction("Create");
            }
            // Get all products belonging to this farmer
            var products = await _context.Products
                .Where(p => p.FarmerId == farmerProfile.Id)
                .ToListAsync();
            // Pass the farmer's name to the view
            ViewBag.FarmerName = farmerProfile.Name;
            return View(products);
        }

        // Displays the form to create a new farmer profile
        public IActionResult Create()
        {
            return View();
        }

        // Handles the submission of the farmer profile creation form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Farmer farmer)
        {
            // Remove UserId from validation since it will be set here
            ModelState.Remove("UserId");
            // Get the current user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError("", "User ID not found. Please try logging in again.");
                System.Diagnostics.Debug.WriteLine("[Create-POST] User ID is null or empty.");
                return View(farmer);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    // Set the UserId and save the new farmer profile
                    farmer.UserId = userId;
                    _context.Add(farmer);
                    await _context.SaveChangesAsync();
                    var addedFarmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == userId);
                    if (addedFarmer != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"[Create-POST] Farmer profile created for user ID: {userId}. Redirecting to Dashboard.");
                        return RedirectToAction(nameof(Dashboard));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Profile was not saved properly.");
                        System.Diagnostics.Debug.WriteLine($"[Create-POST] Profile was not saved properly for user ID: {userId}.");
                    }
                }
                catch (Exception ex)
                {
                    // Handle any errors that occur during save
                    ModelState.AddModelError("", $"Error saving profile: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"[Create-POST] Exception: {ex.Message}");
                }
            }
            else
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"[Create-POST] Model error: {error.ErrorMessage}");
                    }
                }
            }
            return View(farmer);
        }

        // Displays the form to add a new product
        public IActionResult AddProduct()
        {
            return View();
        }

        // Handles the submission of the add product form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(Product product)
        {
            // Remove Farmer from validation since it will be set here
            ModelState.Remove("Farmer");
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            try
            {
                // Get the current user's ID and find the farmer profile
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == userId);
                if (farmer == null)
                {
                    ModelState.AddModelError("", "Farmer profile not found.");
                    return View(product);
                }
                // Set the FarmerId and DateListed, then save the new product
                product.FarmerId = farmer.Id;
                product.DateListed = DateTime.Now;
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                // Show a success message and redirect to dashboard
                TempData["Success"] = "Product added successfully!";
                return RedirectToAction(nameof(Dashboard));
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during save
                ModelState.AddModelError("", "An error occurred while saving: " + ex.Message);
                return View(product);
            }
        }
    }
}