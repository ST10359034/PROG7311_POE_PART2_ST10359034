using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AgriEnergyConnect1.Data;
using AgriEnergyConnect1.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AgriEnergyConnect1.Controllers
{
    [Authorize(Roles = "Farmer")]
    public class FarmerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FarmerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var farmerProfile = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == userId);
            if (farmerProfile == null)
            {
                return RedirectToAction("Create");
            }
            var products = await _context.Products
                .Where(p => p.FarmerId == farmerProfile.Id)
                .ToListAsync();
            ViewBag.FarmerName = farmerProfile.Name;
            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Farmer farmer)
        {
            ModelState.Remove("UserId");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError("", "User ID not found. Please try logging in again.");
                return View(farmer);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    farmer.UserId = userId;
                    _context.Add(farmer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Dashboard));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving: " + ex.Message);
                }
            }
            return View(farmer);
        }

        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(Product product)
        {
            ModelState.Remove("Farmer");
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == userId);
                if (farmer == null)
                {
                    ModelState.AddModelError("", "Farmer profile not found.");
                    return View(product);
                }
                product.FarmerId = farmer.Id;
                product.DateListed = DateTime.Now;
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Product added successfully!";
                return RedirectToAction(nameof(Dashboard));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while saving: " + ex.Message);
                return View(product);
            }
        }
    }
}