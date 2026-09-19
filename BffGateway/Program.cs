using Azure.Security.KeyVault.Secrets;
using Duende.Bff;
using Duende.Bff.EntityFramework;
using Duende.Bff.Yarp;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Add standard Aspire service defaults (includes core service discovery)
builder.AddServiceDefaults(); 

// 1. Add services required for Minimal API Swagger generation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddAzureKeyVaultClient(connectionName: "key-vault");

builder.Services.AddBff()
    .AddServerSideSessions()
    .AddEntityFrameworkServerSideSessions(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("ServerSideSessionsdb"));
    })
    .ConfigureOpenIdConnect(options =>
    {
        options.Authority = "https://demo.duendesoftware.com";
        options.ClientId = "interactive.confidential";
        options.ClientSecret = "secret";
        options.ResponseType = "code";
        options.ResponseMode = "query";

        options.GetClaimsFromUserInfoEndpoint = true;
        options.SaveTokens = true;
        options.MapInboundClaims = false;

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");

        // Add this scope if you want to receive refresh tokens
        options.Scope.Add("offline_access");

        //options.CallbackPath = "/signin-oidc";

        //options.ReturnUrlParameter = "";
    })
    .ConfigureCookies(options =>
    {
        // Because we use an identity server that's configured on a different site
        // (duendesoftware.com vs localhost), we need to configure the SameSite property to Lax.
        // Setting it to Strict would cause the authentication cookie not to be sent after logging in.
        // The user would have to refresh the page to get the cookie.
        // Recommendation: Set it to 'strict' if your IDP is on the same site as your BFF.
        options.Cookie.SameSite = SameSiteMode.Lax;
    });

// Configure what you want to log (Headers, Path, Body, etc.)
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
});

builder.Services.AddReverseProxy()
    .AddBffExtensions()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddServiceDiscoveryDestinationResolver();

builder.Services.AddAuthorization();

// Add `.PersistKeysTo…()` and `.ProtectKeysWith…()` calls
// See more at https://docs.duendesoftware.com/general/data-protection
builder.Services.AddDataProtection()
    .SetApplicationName("BFF");


var app = builder.Build();

// 2. Enable Swagger middleware ONLY in Development environments for safety
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.MapReverseProxy();

app.UseAuthentication();

app.UseHttpLogging();
// Enable endpoint routing, required for the reverse proxy

app.UseRouting();

// adds antiforgery protection for local APIs
app.UseBff();
  
// adds authorization for local and remote API endpoints
app.UseAuthorization();


//---------------

// var secretName = "my-api-key";

// app.MapGet("/my-api-key", async (SecretClient secretClient) =>
// {
//     var secret = await secretClient.GetSecretAsync(secretName);
//     return secret.Value.Value;
// });
app.Run();
