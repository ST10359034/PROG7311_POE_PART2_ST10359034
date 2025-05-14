using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect1.Models
{
    public class Farmer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public string Location { get; set; }

        // Nullable for employee-created farmers
        public string? UserId { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}