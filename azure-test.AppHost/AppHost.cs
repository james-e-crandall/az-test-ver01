var builder = DistributedApplication.CreateBuilder(args);

// Add the following line to configure the Azure App Container environment
builder.AddAzureContainerAppEnvironment("env");

var keyVault = builder.AddAzureKeyVault("key-vault");

var bffGateway = builder.AddProject<Projects.BffGateway>("BffGateway")
    .WithReference(keyVault)
    .WithExternalHttpEndpoints();

var apiKey = builder.AddParameter("api-key", secret: true);
keyVault.AddSecret("my-api-key", apiKey);


builder.Build().Run();
