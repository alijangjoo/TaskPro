namespace Suspect.TaskPro.Endpoints.Api.Extensions.Db;

using Microsoft.EntityFrameworkCore;
using OysterFx.Infra.Persistence.RDB.Commands.Interceptors;
using Suspect.TaskPro.Infra.Persistence.Postgres.Commands;
using Suspect.TaskPro.Infra.Persistence.Postgres.Queries;

public static class DbExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        // Bind configuration
        var connectionStrings = configuration.GetSection("ConnectionStrings").Get<DatabaseConnectionStrings>();

        // Register DbContext with provider switching
        services.AddDbContext<AppCommandDbContext>(options =>
         {
             options.UseNpgsql(connectionStrings!.CommandDb);
             AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
             options.AddInterceptors(new AddAuditDataInterceptor());
         });

        services.AddDbContext<AppQueryDbContext>(options =>
        {
            options.UseNpgsql(connectionStrings!.QueryDb);
        });

        return services;
    }
}