https://docs.duendesoftware.com/bff/fundamentals/session/server-side-sessions/

https://docs.duendesoftware.com/bff/getting-started/single-frontend/



To create the required database tables for Duende BFF Server-Side Sessions using Entity Framework Core, you need to generate and apply an EF Core migration targeting the internal SessionDbContext provided by Duende.

Duende BFF deliberately does not automatically execute database scripts or create tables on startup. Follow these steps to generate and run the migration:

1. Ensure Dependencies and Configuration are Set

Make sure you have installed the Entity Framework package:

dotnet add package Duende.BFF.EntityFramework


Your Program.cs should be configured to use EF for session storage:

builder.Services.AddBff()
    .AddServerSideSessions()
    .AddEntityFrameworkServerSideSessions(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

2. Generate the Migration

Run the following command in your terminal from the directory containing your startup project. You must explicitly point to Duende's context (SessionDbContext) using the -c flag:

dotnet ef migrations add AddBFFUserSessions -c SessionDbContext -o Migrations

-c SessionDbContext: Directs the tooling to use Duende's session database context.

-o Migrations: Specifies the directory where the migration files will be saved.

3. Apply the Migration (Create Tables)

To execute the migration and create the UserSessions tables in your target database, run:

dotnet ef database update -c SessionDbContext

Alternative: Generating the SQL Script

If your organization prefers to apply schema changes manually or via a CI/CD pipeline rather than running dotnet ef database update directly, you can generate the raw SQL script:

dotnet ef migrations script -c SessionDbContext




