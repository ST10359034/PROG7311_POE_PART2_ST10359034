# Agri-Energy Connect1 Prototype

## Project Overview
Agri-Energy Connect1 is a prototype web application designed to connect the agricultural sector with green energy technology providers. The platform enables farmers and employees to collaborate, manage products, and access resources for sustainable agriculture and renewable energy.

## Project Structure

- **AgriEnergyConnect1.sln**: Visual Studio solution file. Open this to load the project in Visual Studio.
- **AgriEnergyConnect1.csproj**: Project file for the main application.
- **Controllers/**, **Models/**, **Views/**, **ViewModels/**: MVC structure for the application.
- **Data/**: Contains the database context and seed data logic.
- **Migrations/**: Entity Framework Core migration scripts. These allow you to create the database schema automatically.
- **wwwroot/**: Static files (CSS, JS, etc.).
- **appsettings.json**: Configuration file for database connection and other settings.
- **README.md**: This documentation.

## Database Setup

**You do NOT need to use the included `.db` files. Instead, follow these steps to create your own database:**

1. **Open the Solution**
   - Double-click `AgriEnergyConnect1.sln` to open the project in Visual Studio 2022 or later.

2. **Restore NuGet Packages**
   - Visual Studio will prompt you, or right-click the solution and select "Restore NuGet Packages".

3. **Apply Migrations to Create the Database**
   - Open the **Package Manager Console** (Tools > NuGet Package Manager > Package Manager Console).
   - Make sure the **Default Project** (dropdown at the top) is set to `AgriEnergyConnect1`.
   - Run:
     ```
     Update-Database
     ```
   - This will use the scripts in the `Migrations` folder to create a new SQLite database (`AgriEnergyConnect.db`) with all tables and seed data.

4. **Run the Application**
   - Press `F5` or click the "Run" button in Visual Studio.

## Notes

- **Do not edit or use the `.db`, `.db-shm`, or `.db-wal` files directly.** They are generated automatically and can be deleted if you want to start fresh.
- **If you encounter errors:**  
  - Ensure you have the latest version of Visual Studio and the .NET SDK installed.
  - Make sure you have write permissions in the project directory.

## Quick Start Checklist

- [ ] Open `AgriEnergyConnect1.sln` in Visual Studio
- [ ] Restore NuGet packages
- [ ] Run `Update-Database` in the Package Manager Console
- [ ] Press `F5` to run the app

---

If you follow these steps, you will be able to open, build, and run the project with no issues!

## System Functionalities and User Roles

### User Roles

- **Farmer**
  - Register and log in as a Farmer.
  - Add and view their own agricultural products.
- **Employee**
  - Register and log in as an Employee.
  - Add and view farmer profiles.
  - View and filter all products listed by farmers.

### Key Functionalities

- **Authentication & Authorization**
  - Secure login system with role-based access (Farmer, Employee).
- **Product Management**
  - Farmers can add, view, and manage their products.
- **Profile Management**
  - Employees can add and view farmer profiles.
- **Product Browsing**
  - Employees can view and filter all products.
- **Responsive UI**
  - User-friendly interface built with Bootstrap.
- **Data Validation**
  - All forms include validation for data integrity.

---

## Additional Notes

- Sample data is available in `AgriEnergyConnect1/Data/SeedData.cs` for quick testing.
- The application uses SQLite by default but can be configured for SQL Server LocalDB.
- For any issues, ensure your database connection string is correct and all migrations are applied.

---

## License

For educational use only. 