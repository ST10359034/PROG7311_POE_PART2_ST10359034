using Microsoft.AspNetCore.Mvc;
using AgriEnergyConnect1.Models;
using System.Diagnostics;

namespace AgriEnergyConnect1.Controllers
{
    // This controller manages general site navigation and error handling
    public class HomeController : Controller
    {
        // Logger for tracking application events and errors
        private readonly ILogger<HomeController> _logger;

        // Constructor injects the logger
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Displays the home page
        public IActionResult Index()
        {
            return View();
        }

        // Displays the privacy policy page
        public IActionResult Privacy()
        {
            return View();
        }

        // Handles and displays error information
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Create an error view model with the current request ID
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}