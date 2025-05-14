using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AgriEnergyConnect1.Data;
using AgriEnergyConnect1.Models;
using Microsoft.AspNetCore.Identity.UI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Get the connection string from configuration and set up the database context to use SQLite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// Add a developer exception filter for database-related errors
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configure Identity for authentication and authorization, including roles
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add support for MVC controllers and views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // In development, you can add developer-specific middleware here
}
else
{
    // In production, use a custom error handler and enable HSTS for security
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Redirect HTTP requests to HTTPS and serve static files (like CSS, JS, images)
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Set up the default route for MVC controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Enable Razor Pages (for Identity UI, etc.)
app.MapRazorPages();

// Ensure required roles exist in the database at startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Get the RoleManager service
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        // Define the roles needed for the application
        string[] roleNames = { "Farmer", "Employee" };
        foreach (var roleName in roleNames)
        {
            // Create the role if it does not already exist
            if (!roleManager.RoleExistsAsync(roleName).Result)
            {
                roleManager.CreateAsync(new IdentityRole(roleName)).Wait();
            }
        }
    }
    catch (Exception ex)
    {
        // Log any errors that occur during role initialization
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while initializing roles.");
    }
}

// Start the application
app.Run(); ;