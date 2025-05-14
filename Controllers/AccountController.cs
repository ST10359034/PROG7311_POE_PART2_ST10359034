using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AgriEnergyConnect1.Models;
using AgriEnergyConnect1.ViewModels;
using System.Threading.Tasks;

namespace AgriEnergyConnect1.Controllers
{
    // This controller manages user authentication and registration
    public class AccountController : Controller
    {
        // UserManager and SignInManager are used for handling user accounts and sign-in operations
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        // Constructor injects the required services
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Displays the login page
        public IActionResult Login()
        {
            return View();
        }

        // Handles login form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Check if the form data is valid
            if (ModelState.IsValid)
            {
                // Attempt to sign in the user with the provided credentials
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    // If login is successful, find the user by email
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user != null)
                    {
                        // Redirect user to their respective dashboard based on their role
                        if (await _userManager.IsInRoleAsync(user, "Farmer"))
                        {
                            return RedirectToAction("Dashboard", "Farmer");
                        }
                        else if (await _userManager.IsInRoleAsync(user, "Employee"))
                        {
                            return RedirectToAction("Dashboard", "Employee");
                        }
                    }
                    // If user role is not found, redirect to home page
                    return RedirectToAction("Index", "Home");
                }
                // If login fails, show an error message
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            // If model state is invalid or login fails, return the view with errors
            return View(model);
        }

        // Displays the registration page
        public IActionResult Register()
        {
            return View();
        }

        // Handles registration form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // Check if the form data is valid
            if (ModelState.IsValid)
            {
                // Create a new user with the provided email and password
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Assign the selected role to the user
                    await _userManager.AddToRoleAsync(user, model.Role);
                    // Automatically sign in the user after registration
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Redirect user to the appropriate creation page based on their role
                    if (model.Role == "Farmer")
                    {
                        return RedirectToAction("Create", "Farmer");
                    }
                    else if (model.Role == "Employee")
                    {
                        return RedirectToAction("Create", "Employee");
                    }

                    // If role is not recognized, redirect to home page
                    return RedirectToAction("Index", "Home");
                }

                // If registration fails, display all error messages
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            // If model state is invalid or registration fails, return the view with errors
            return View(model);
        }

        // Handles user logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Sign out the current user
            await _signInManager.SignOutAsync();
            // Redirect to the home page after logout
            return RedirectToAction("Index", "Home");
        }
    }
}