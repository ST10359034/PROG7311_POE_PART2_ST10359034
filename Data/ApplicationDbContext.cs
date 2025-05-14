using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AgriEnergyConnect1.Models;

namespace AgriEnergyConnect1.Data
{
    // This class represents the Entity Framework database context for the application,
    // including identity (user) management and custom entities.
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // The constructor receives configuration options and passes them to the base class.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Represents the Farmers table in the database.
        public DbSet<Farmer> Farmers { get; set; }

        // Represents the Employees table in the database.
        public DbSet<Employee> Employees { get; set; }

        // Represents the Products table in the database.
        public DbSet<Product> Products { get; set; }
    }
}