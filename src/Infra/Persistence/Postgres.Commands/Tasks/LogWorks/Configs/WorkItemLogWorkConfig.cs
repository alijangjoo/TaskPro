namespace Suspect.TaskPro.Infra.Persistence.Postgres.Commands.Tasks.LogWorks.Configs;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Suspect.TaskPro.AppCore.Domain.TaskAggregate.Entities;

public class WorkItemLogWorkConfig : IEntityTypeConfiguration<WorkItemLogWork>
{
    public void Configure(EntityTypeBuilder<WorkItemLogWork> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.WorkItemBusinessKey)
               .HasConversion<string>()
               .HasMaxLength(200);

        builder.Property(x => x.ResourceBusinessKey)
               .HasConversion<string>()
               .HasMaxLength(200);

        builder.Property(x => x.Description)
               .HasMaxLength(2000);

        builder.Property(x => x.LogType)
               .HasConversion<string>()
               .HasMaxLength(100);

        builder.Property(x => x.Notes)
               .HasMaxLength(1000);
    }
}