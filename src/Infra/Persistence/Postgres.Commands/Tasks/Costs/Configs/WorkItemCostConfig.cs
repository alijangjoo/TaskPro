namespace Suspect.TaskPro.Infra.Persistence.Postgres.Commands.Tasks.Costs.Configs;

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Suspect.TaskPro.AppCore.Domain.TaskAggregate.Entities;

public class WorkItemCostConfig : IEntityTypeConfiguration<WorkItemCost>
{
    public void Configure(EntityTypeBuilder<WorkItemCost> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.WorkItemBusinessKey)
               .HasConversion<string>()
               .HasMaxLength(200);

        builder.Property(x => x.Type)
               .HasConversion<string>()
               .HasMaxLength(100);

        builder.Property(x => x.Amount)
               .HasPrecision(18, 2);

        builder.Property(x => x.Currency)
               .HasMaxLength(10)
               .HasDefaultValue("IRR");

        builder.Property(x => x.Description)
               .HasMaxLength(1000);

        builder.Property(x => x.Status)
               .HasConversion<string>()
               .HasMaxLength(50);

        builder.HasOne(x => x.WorkItem)
               .WithMany(w => w.Costs)
               .HasForeignKey("WorkItemId");

        builder.HasMany(x => x.Attachments)
               .WithOne()
               .HasForeignKey("CostId");
    }
}