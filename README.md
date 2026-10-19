
🔐 Database & ASP.NET Core Identity Module
A core backend infrastructure module built using ASP.NET Core MVC and EF Core as part of the Blood Donation & Management System, responsible for database context management, user authentication, identity extensions, and database seeding.

📋 Detailed Tasks & Features
- Database Context & Configuration (ApplicationDbContext): Setting up Entity Framework Core DbContext inheriting from IdentityDbContext to manage core application entities and identity tables.
- Custom Identity User Model (ApplicationUser): Extending standard IdentityUser to support custom donor and user profile properties.
- Database Initialization & Seeding (DbInitializer): Implementing database seeding logic for automated creation of system roles, default settings, and initial data.
- Service & Middleware Configuration (Program.cs): Registering EF Core connection strings, Identity authentication services, and executing database initializer upon startup.
- EF Core Migrations Management: Generating and managing initial database schema migrations (InitialIdentitySetup, UpdateIdentityAndSeedData).

🛠️ Tech Stack & Architecture
- Framework: .NET 8.0 / ASP.NET Core MVC
- Identity & ORM: ASP.NET Core Identity, Entity Framework Core (EF Core), SQL Server
- Architecture: Clean Architecture / N-Tier (Data Layer, Models, Migrations)
- Tools: Visual Studio / VS Code, Git / GitHub

👨‍💻 Author & Developer
- Developer: Mohammad Sameh