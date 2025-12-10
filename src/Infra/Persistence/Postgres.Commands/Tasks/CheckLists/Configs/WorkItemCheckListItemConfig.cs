namespace Suspect.TaskPro.Infra.Persistence.Postgres.Commands.Tasks.CheckLists.Configs;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Suspect.TaskPro.AppCore.Domain.TaskAggregate.Entities;

public class WorkItemCheckListItemConfig : IEntityTypeConfiguration<WorkItemCheckListItem>
{
    public void Configure(EntityTypeBuilder<WorkItemCheckListItem> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.WorkItemCheckListBusinessKey)
               .HasConversion<string>()
               .HasMaxLength(200);

        builder.Property(x => x.CompletedByResourceBusinessKey)
               .HasConversion<string>()
               .HasMaxLength(200);

        builder.Property(x => x.Title)
               .HasMaxLength(300)
               .IsRequired();

        builder.Property(x => x.Description)
               .HasMaxLength(1000);

        builder.Property(x => x.IsCompleted)
               .HasDefaultValue(false);

        builder.Property(x => x.Order)
               .HasDefaultValue(0);
    }
}