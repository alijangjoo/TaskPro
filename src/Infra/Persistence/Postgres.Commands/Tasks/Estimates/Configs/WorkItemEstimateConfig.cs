namespace Suspect.TaskPro.Infra.Persistence.Postgres.Commands.Tasks.Estimates.Configs;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Suspect.TaskPro.AppCore.Domain.TaskAggregate.Entities;

public class WorkItemEstimateConfig : IEntityTypeConfiguration<WorkItemEstimate>
{
    public void Configure(EntityTypeBuilder<WorkItemEstimate> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.EstimatedByResourceBusinessKey)
               .HasConversion<string>()
               .HasMaxLength(200);

        builder.Property(x => x.Assumptions)
               .HasMaxLength(1000);

        builder.Property(x => x.Type)
               .HasConversion<string>()
               .HasMaxLength(100);
    }
}