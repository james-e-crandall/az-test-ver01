var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

HomeEndpoints.RegisterEndpoints(app);

app.Run();
