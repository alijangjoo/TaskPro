namespace Suspect.TaskPro.Infra.Persistence.Postgres.Queries.Tasks.Configs;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Suspect.TaskPro.Infra.Persistence.Postgres.Queries.Tasks.Entities;

public class WorkItemConfig : IEntityTypeConfiguration<WorkItem>
{
    public void Configure(EntityTypeBuilder<WorkItem> builder)
    {
        builder.ToTable("WorkItems");
    }
}