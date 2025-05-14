using System;
using System.Linq;
using AgriEnergyConnect1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AgriEnergyConnect1.Data
{
    // This static class is responsible for seeding the database with initial data
    public static class SeedData
    {
        // This method populates the database with sample farmers, employees, and products if not already present
        public static void Initialize(IServiceProvider serviceProvider)
        {
            // Create a new database context using the provided service provider
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Check if there are any farmers in the database already
                if (context.Farmers.Any())
                {
                    // If farmers exist, assume the database has already been seeded and exit
                    return;   // DB has been seeded
                }

                // Create sample farmer records
                var farmer1 = new Farmer { Name = "John Doe", Email = "john@farm.com", Location = "Eastern Cape" };
                var farmer2 = new Farmer { Name = "Mary Smith", Email = "mary@farm.com", Location = "Western Cape" };
                context.Farmers.AddRange(farmer1, farmer2);

                // Create sample employee records
                var employee1 = new Employee { Name = "Alice Green", Email = "alice@energy.com", Department = "Solar" };
                var employee2 = new Employee { Name = "Bob Brown", Email = "bob@energy.com", Department = "Wind" };
                context.Employees.AddRange(employee1, employee2);

                // Save changes to generate IDs for farmers and employees
                context.SaveChanges();

                // Create sample product records linked to the farmers
                context.Products.AddRange(
                    new Product
                    {
                        Name = "Organic Tomatoes",
                        Category = "Vegetables",
                        Price = 25.99m,
                        Quantity = 100,
                        ProductionDate = DateTime.Now.AddDays(-10),
                        DateListed = DateTime.Now.AddDays(-5),
                        FarmerId = farmer1.Id
                    },
                    new Product
                    {
                        Name = "Free-range Eggs",
                        Category = "Poultry",
                        Price = 45.50m,
                        Quantity = 50,
                        ProductionDate = DateTime.Now.AddDays(-5),
                        DateListed = DateTime.Now.AddDays(-3),
                        FarmerId = farmer2.Id
                    },
                    new Product
                    {
                        Name = "Solar Pump",
                        Category = "Equipment",
                        Price = 1200.00m,
                        Quantity = 5,
                        ProductionDate = DateTime.Now.AddDays(-15),
                        DateListed = DateTime.Now.AddDays(-2),
                        FarmerId = farmer1.Id
                    }
                );

                // Save all changes to the database
                context.SaveChanges();
            }
        }
    }
}