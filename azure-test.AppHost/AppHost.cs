var builder = DistributedApplication.CreateBuilder(args);

// Add the following line to configure the Azure App Container environment
builder.AddAzureContainerAppEnvironment("env");

var sqlServer = builder.AddSqlServer("sqlServer")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDbGate();

var serverSideSessionsdb = sqlServer.AddDatabase("ServerSideSessionsdb");

var keyVault = builder.AddAzureKeyVault("key-vault");

var frontend = builder.AddJavaScriptApp("frontend", "../frontend", runScriptName: "start")
    .WithNpm(installCommand: "ci")
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

var remoteApi = builder.AddProject<Projects.RemoteApi>("RemoteApi")
    .WithReference(keyVault);

var bffGateway = builder.AddProject<Projects.BffGateway>("BffGateway")
    .WithReference(keyVault)
    .WithReference(frontend)
    .WaitFor(serverSideSessionsdb)
    .WithReference(serverSideSessionsdb)
    .WithExternalHttpEndpoints()
    .WithReference(remoteApi);

var apiKey = builder.AddParameter("api-key", secret: true);
keyVault.AddSecret("my-api-key", apiKey);

frontend.WithReference(bffGateway)
    .WaitFor(bffGateway);

builder.Build().Run();
