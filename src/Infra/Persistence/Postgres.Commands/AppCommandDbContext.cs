namespace Suspect.TaskPro.Infra.Persistence.Postgres.Commands;

using Microsoft.EntityFrameworkCore;
using OysterFx.Infra.Persistence.RDB.Commands;
using Suspect.TaskPro.AppCore.Domain.AAA.Entities;
using Suspect.TaskPro.AppCore.Domain.OtpCodesAggregate.Entities;
using Suspect.TaskPro.AppCore.Domain.ResourcesAggregate.Entities;
using Suspect.TaskPro.AppCore.Domain.TaskAggregate.Entities;
using Suspect.TaskPro.AppCore.Domain.WrapperAggregate.Entities;

public class AppCommandDbContext : CommandDbContext
{
    public DbSet<WorkItem> WorkItems { get; set; }
    public DbSet<Resource> Resources { get; set; }
    public DbSet<Wrapper> Wrappers { get; set; }
    public DbSet<OtpCode> OtpCodes { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public AppCommandDbContext(DbContextOptions<AppCommandDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
