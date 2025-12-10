namespace Suspect.TaskPro.Infra.Persistence.Postgres.Queries.Tasks.Configs;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Suspect.TaskPro.Infra.Persistence.Postgres.Queries.Tasks.Entities;

public class WorkItemScheduleConfig : IEntityTypeConfiguration<WorkItemSchedule>
{
    public void Configure(EntityTypeBuilder<WorkItemSchedule> builder)
    {
        builder.ToTable("WorkItemSchedule");
    }
}