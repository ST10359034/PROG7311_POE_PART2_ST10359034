using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect1.ViewModels
{
    // ViewModel used for user registration, including validation rules for each field
    public class RegisterViewModel
    {
        // The user's email address (required, must be a valid email format)
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        // The user's password (required, must be at least 6 characters and at most 100)
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        // Confirmation of the password (must match the Password field)
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        // The role the user is registering as (e.g., Farmer or Employee)
        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }
    }
}