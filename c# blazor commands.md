# EF Core + Razor Pages + Code Generation Commands

This document contains useful .NET CLI commands for setting up ASP.NET Core projects, Entity Framework Core, and Razor Pages scaffolding.

---

# 1. Install / Update Code Generation Tools

```bash
# Uninstall existing tools
dotnet tool uninstall --global dotnet-aspnet-codegenerator
dotnet tool uninstall --global dotnet-ef

# Install tools again
dotnet tool install --global dotnet-aspnet-codegenerator
dotnet tool install --global dotnet-ef
```

---

# 2. Add Required NuGet Packages

## Entity Framework Core + Code Generation

```bash
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SQLite
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

## Specific EF Core Version (Optional)

```bash
dotnet add package Microsoft.EntityFrameworkCore --version 9.0.0
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 9.0.0
dotnet add package System.Net.Http.Json --version 9.0.0
```

---

# 3. Create Razor Pages Project

```bash
dotnet new webapp -o RazorPagesMovie
code -r RazorPagesMovie
```

---

# 4. Scaffold Razor Pages (Code Generator)

```bash
dotnet aspnet-codegenerator razorpage \
-m Movie \
-dc RazorPagesMovie.Data.RazorPagesMovieContext \
-udl \
-outDir Pages/Movies \
--referenceScriptLibraries \
--databaseProvider sqlite
```

---

# 5. Scaffold Options Explained

| Option | Description |
|------|-------------|
| `-m` | Name of the model class |
| `-dc` | DbContext class (including namespace) |
| `-udl` | Use default layout |
| `-outDir` | Output folder for generated pages |
| `--referenceScriptLibraries` | Adds validation scripts to Create/Edit pages |
| `--databaseProvider` | Database provider (e.g. sqlite) |

---

# 6. Entity Framework Core Commands

## Create migrations
```bash
dotnet ef migrations add InitialCreate
```

## Apply migrations (create database)
```bash
dotnet ef database update
```

---

# 7. Quick Help

```bash
dotnet aspnet-codegenerator razorpage -h
```

---

# 8. Notes

- Always run `dotnet restore` after adding packages
- Ensure `dotnet-ef` is installed globally before migrations
- Use SQLite for simple development setups



# // Initialize the database
- var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
- using (var scope = scopeFactory.CreateScope())
- {
    var db = scope.ServiceProvider.GetRequiredService<PizzaStoreContext>();
    if (db.Database.EnsureCreated())
    {
        SeedData.Initialize(db);
    }
- }