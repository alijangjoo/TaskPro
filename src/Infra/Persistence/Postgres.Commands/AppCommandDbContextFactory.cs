namespace Suspect.TaskPro.Infra.Persistence.Postgres.Commands;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql;

public class AppCommandDbContextFactory : IDesignTimeDbContextFactory<AppCommandDbContext>
{
    public AppCommandDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppCommandDbContext>();

        optionsBuilder.UseNpgsql(
            "Host=localhost;Database=TaskProDb;Username=postgresuser;Password=2wsx@WSX123");

        return new AppCommandDbContext(optionsBuilder.Options);
    }
}
