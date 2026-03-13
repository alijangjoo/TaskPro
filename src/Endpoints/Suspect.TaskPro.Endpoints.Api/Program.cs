using Suspect.TaskPro.Endpoints.Api;
using Suspect.TaskPro.Infra.Persistence.Postgres.Commands;

var builder = WebApplication.CreateBuilder(args);
var app = builder.ConfigureServices().ConfigurePipeline();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppCommandDbContext>();
        context.Database.EnsureCreated(); // Creates database and tables if they don't exist
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

app.Run();