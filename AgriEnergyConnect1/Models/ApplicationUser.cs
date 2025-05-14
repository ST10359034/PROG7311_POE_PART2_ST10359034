using Microsoft.AspNetCore.Identity;

namespace AgriEnergyConnect1.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Remove the Role property - ASP.NET Identity manages roles through its own tables
        // Identity will use AspNetRoles and AspNetUserRoles tables instead
    }
}