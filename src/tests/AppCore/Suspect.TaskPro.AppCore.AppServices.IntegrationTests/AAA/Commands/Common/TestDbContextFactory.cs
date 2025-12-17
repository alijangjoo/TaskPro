namespace Suspect.TaskPro.AppCore.AppServices.IntegrationTests.AAA.Commands.Common;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OysterFx.Infra.Auth.UserServices;
using Suspect.TaskPro.Infra.Persistence.Postgres.Commands;

public static class TestDbContextFactory
{
    public static AppCommandDbContext Create()
    {
        var services = new ServiceCollection();
        services.AddScoped<IUserInfoService, FakeUserInfoService>();

        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        services.AddDbContext<AppCommandDbContext>(options =>
        {
            options.UseSqlite(connection);
        });

        var provider = services.BuildServiceProvider();
        var context = provider.GetRequiredService<AppCommandDbContext>();

        context.Database.EnsureCreated();

        return context;
    }
}