namespace Suspect.TaskPro.Infra.Persistence.Postgres.Commands.Tasks.Costs.Configs;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Suspect.TaskPro.AppCore.Domain.TaskAggregate.Entities;

public class WorkItemCostAttachmentConfig : IEntityTypeConfiguration<WorkItemCostAttachment>
{
    public void Configure(EntityTypeBuilder<WorkItemCostAttachment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CostBusinessKey)
               .HasConversion<string>()
               .HasMaxLength(200);

        builder.Property(x => x.FileName)
               .HasMaxLength(500);

        builder.Property(x => x.FilePath)
               .HasMaxLength(1000);

        builder.Property(x => x.ContentType)
               .HasMaxLength(200);

        builder.Property(x => x.Description)
               .HasMaxLength(1000);
    }
}