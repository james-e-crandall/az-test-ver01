using BffGateway.MigrationService;
using Duende.Bff.EntityFramework;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

//builder.AddSqlServerDbContext<SessionDbContext>("ServerSideSessionsdb");

builder.Services.AddDbContext<SessionDbContext>(options =>
{
     options.UseSqlServer(builder.Configuration.GetConnectionString("ServerSideSessionsdb"), b => b.MigrationsAssembly("BffGateway.MigrationService"));
});



var host = builder.Build();
host.Run();
