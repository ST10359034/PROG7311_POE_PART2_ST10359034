# AgriEnergyConnect1

## Getting Started from GitHub

This guide will help you clone, set up, and run the AgriEnergyConnect1 project from GitHub.

---

## 1. Clone the Repository

1. Go to the GitHub repository: [https://github.com/ST10359034/PROG7311_POE_PART2_ST10359034](https://github.com/ST10359034/PROG7311_POE_PART2_ST10359034)
2. Click the green **Code** button and copy the HTTPS URL.
3. Open a terminal or Git Bash and run:
   ```sh
   git clone https://github.com/ST10359034/PROG7311_POE_PART2_ST10359034.git
   ```
4. Navigate into the project folder:
   ```sh
   cd PROG7311_POE_PART2_ST10359034/AgriEnergyConnect1
   ```

---

## 2. Open the Project in Visual Studio

1. Open Visual Studio 2022 or later.
2. Click **Open a project or solution** and select `AgriEnergyConnect1.sln` from the `AgriEnergyConnect1` folder inside the cloned repo.

---

## 3. Restore NuGet Packages

- Visual Studio will usually prompt you to restore NuGet packages. If not, right-click the solution in Solution Explorer and select **Restore NuGet Packages**.

---

## 4. Set Up the Database

1. Open the **Package Manager Console** (Tools > NuGet Package Manager > Package Manager Console).
2. Make sure the **Default Project** (dropdown at the top) is set to `AgriEnergyConnect1`.
3. Run:
   ```sh
   Update-Database
   ```
   This will use the Entity Framework Core migrations to create a new SQLite database (`AgriEnergyConnect.db`) with all tables and seed data.

---

## 5. Run the Application

- Press `F5` or click the **Run** button in Visual Studio.
- The app will open in your browser at `https://localhost:...`.

---

## Project Structure

- **AgriEnergyConnect1.sln**: Visual Studio solution file.
- **AgriEnergyConnect1.csproj**: Project file for the main application.
- **Controllers/**, **Models/**, **Views/**, **ViewModels/**: MVC structure for the application.
- **Data/**: Database context and seed data logic.
- **Migrations/**: Entity Framework Core migration scripts.
- **wwwroot/**: Static files (CSS, JS, etc.).
- **appsettings.json**: Configuration file for database connection and other settings.
- **README.md**: This documentation.

---

## Troubleshooting

- **Always extract the project from a zip before opening in Visual Studio.**
- **If you get path errors, move the project to a short path (e.g., `C:\Test\AgriEnergyConnect1`).**
- **If the database does not generate, make sure you ran `Update-Database` in the Package Manager Console.**
- **If you see missing files or errors, ensure you cloned the full repository and restored NuGet packages.**

---

## Quick Start Checklist

- [ ] Clone the repository from GitHub
- [ ] Open `AgriEnergyConnect1.sln` in Visual Studio
- [ ] Restore NuGet packages
- [ ] Run `Update-Database` in the Package Manager Console
- [ ] Press `F5` to run the app

---

## License

For educational use only. 
