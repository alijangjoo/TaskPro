namespace Suspect.TaskPro.AppCore.AppServices.IntegrationTests.AAA.Commands.Common;
using Testcontainers.PostgreSql;

public class PostgresFixture : IAsyncLifetime
{
    public PostgreSqlContainer Container { get; }
    public PostgresFixture()
    {
        Container = new PostgreSqlBuilder()
            .WithDatabase("taskpro_test")
            .WithUsername("postgresuser")
            .WithPassword("2wsx@WSX123")
            .Build();

        Container.StartAsync();
    }

    public async Task DisposeAsync() => await Container.StopAsync();

    public async Task InitializeAsync() => await Container.StartAsync();
}