namespace Suspect.TaskPro.Infra.Persistence.Postgres.Queries;

using Microsoft.EntityFrameworkCore;
using OysterFx.Infra.Auth.UserServices;
using OysterFx.Infra.Persistence.RDB.Queries;
using Suspect.TaskPro.Infra.Persistence.Postgres.Queries.Tasks.Entities;

public class AppQueryDbContext : QueryDbContext
{
    private readonly IUserInfoService _userInfoService;

    public DbSet<WorkItem> WorkItems { get; set; }
    public DbSet<WorkItemSchedule> WorkItemSchedule { get; set; }
    public AppQueryDbContext(DbContextOptions<AppQueryDbContext> options, IUserInfoService userInfoService) : base(options)
    {
        _userInfoService = userInfoService;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkItem>().HasQueryFilter(w => w.CreatedByUserId == _userInfoService.UserId);
    }
}