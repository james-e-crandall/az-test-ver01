

public static class HomeEndpoints
{
    public static void RegisterEndpoints(this WebApplication app)
    {
        var homeItems = app.MapGroup("/home");

        homeItems.MapGet("/hello-world", () => "hello-world");
    }
}