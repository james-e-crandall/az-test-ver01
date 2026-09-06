using Azure.Security.KeyVault.Secrets;

var builder = WebApplication.CreateBuilder(args);

builder.AddAzureKeyVaultClient(connectionName: "key-vault");

var app = builder.Build();

var secretName = "my-api-key";

app.MapGet("/", async (SecretClient secretClient) =>
{
        var secret = await secretClient.GetSecretAsync(secretName);
        return secret.Value.Value;
});

app.Run();
