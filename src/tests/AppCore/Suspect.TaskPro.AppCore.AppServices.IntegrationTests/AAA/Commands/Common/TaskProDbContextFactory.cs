namespace Suspect.TaskPro.AppCore.AppServices.IntegrationTests.AAA.Commands.Common;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OysterFx.Infra.Auth.UserServices;
using Suspect.TaskPro.Infra.Persistence.Postgres.Commands;

public static class TaskProDbContextFactory
{
    public static AppCommandDbContext Create(string connectionString)
    {
        var services = new ServiceCollection();

        services.AddScoped<IUserInfoService, FakeUserInfoService>();

        services.AddDbContext<AppCommandDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        var provider = services.BuildServiceProvider();
        var context = provider.GetRequiredService<AppCommandDbContext>();

        context.Database.Migrate();
        return context;
    }
}