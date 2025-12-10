namespace Suspect.TaskPro.Infra.Persistence.Postgres.Commands.Tasks.CheckLists.Configs;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Suspect.TaskPro.AppCore.Domain.TaskAggregate.Entities;

public class WorkItemCheckListConfig : IEntityTypeConfiguration<WorkItemCheckList>
{
    public void Configure(EntityTypeBuilder<WorkItemCheckList> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.WorkItemBusinessKey)
               .HasConversion<string>()
               .HasMaxLength(200);

        builder.Property(x => x.Title)
               .HasMaxLength(300)
               .IsRequired();

        builder.Property(x => x.Description)
               .HasMaxLength(1000);

        builder.Property(x => x.IsDeleted)
               .HasDefaultValue(false);

        builder.HasMany(x => x.Items)
               .WithOne()
               .HasForeignKey("CheckListId");

        builder.Navigation(x => x.Items).AutoInclude();
    }
}
