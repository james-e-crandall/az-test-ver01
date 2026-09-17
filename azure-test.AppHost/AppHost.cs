var builder = DistributedApplication.CreateBuilder(args);

// Add the following line to configure the Azure App Container environment
builder.AddAzureContainerAppEnvironment("env");

var keyVault = builder.AddAzureKeyVault("key-vault");

var remoteApi = builder.AddProject<Projects.RemoteApi>("RemoteApi")
    .WithReference(keyVault)
    .WithExternalHttpEndpoints();

var bffGateway = builder.AddProject<Projects.BffGateway>("BffGateway")
    .WithReference(keyVault)
    .WithExternalHttpEndpoints()
    .WithReference(remoteApi);

var apiKey = builder.AddParameter("api-key", secret: true);
keyVault.AddSecret("my-api-key", apiKey);



builder.Build().Run();
