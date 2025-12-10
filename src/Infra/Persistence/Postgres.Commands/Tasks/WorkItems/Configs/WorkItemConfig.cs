namespace Suspect.TaskPro.Infra.Persistence.Postgres.Commands.Tasks.WorkItems.Configs;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Suspect.TaskPro.AppCore.Domain.TaskAggregate.Entities;

public class WorkItemConfig : IEntityTypeConfiguration<WorkItem>
{
    public void Configure(EntityTypeBuilder<WorkItem> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
               .HasMaxLength(300);

        builder.Property(x => x.Description)
               .HasMaxLength(2000);

        builder.Property(x => x.Status)
               .HasConversion<string>()
               .HasMaxLength(50);

        builder.Property(x => x.HasAlarm)
       .HasColumnName("has_alarm")
       .IsRequired();

        builder.Property(x => x.AlarmTime)
               .HasColumnType("time")
               .HasColumnName("alarm_time")
               .IsRequired(false);

        //builder.Property(x => x.WrapperBusinessKey)
        //       .HasConversion<string>()
        //       .HasMaxLength(200)
        //       .IsRequired(false);

        builder.Property(x => x.ParentId).IsRequired(false);

        builder.OwnsOne(x => x.Estimate, estimate =>
        {
            estimate.Property(e => e.OptimisticHours)
                   .HasColumnName("Estimate_OptimisticHours");

            estimate.Property(e => e.MostLikelyHours)
                   .HasColumnName("Estimate_MostLikelyHours");

            estimate.Property(e => e.PessimisticHours)
                   .HasColumnName("Estimate_PessimisticHours");

            estimate.Property(e => e.ConfidenceLevel)
                   .HasColumnName("Estimate_ConfidenceLevel");

            estimate.Property(e => e.EstimateDate)
                   .HasColumnName("Estimate_Date");

            estimate.Property(e => e.EstimatedByResourceBusinessKey)
                   .HasColumnName("Estimate_ByResourceKey")
                   .HasConversion<string>()
                   .HasMaxLength(200);

            estimate.Property(e => e.Assumptions)
                   .HasColumnName("Estimate_Assumptions")
                   .HasMaxLength(1000);

            estimate.Property(e => e.Type)
                   .HasColumnName("Estimate_Type")
                   .HasConversion<string>();
        });

        builder.OwnsMany(x => x.Schedules, schedule =>
        {
            schedule.ToTable("WorkItemSchedules");

            schedule.WithOwner().HasForeignKey("WorkItemId");

            schedule.Property<long>("Id");
            schedule.HasKey("Id");

            schedule.Property(e => e.IsDone);
            schedule.Property(e => e.ScheduledDate);

            schedule.Property(c => c.BusinessKey)
                .HasConversion<string>()
                .HasMaxLength(200);
        });

        builder.OwnsMany(x => x.EstimateHistory, history =>
        {
            history.ToTable("WorkItemEstimateHistory");

            history.WithOwner().HasForeignKey("WorkItemId");

            history.Property<long>("Id");
            history.HasKey("Id");

            history.Property(e => e.OptimisticHours);
            history.Property(e => e.MostLikelyHours);
            history.Property(e => e.PessimisticHours);
            history.Property(e => e.ConfidenceLevel);
            history.Property(e => e.EstimateDate);

            history.Property(e => e.EstimatedByResourceBusinessKey)
                   .HasConversion<string>()
                   .HasMaxLength(200);

            history.Property(e => e.Assumptions).HasMaxLength(1000);
            history.Property(e => e.Type).HasConversion<string>();
        });

        builder.OwnsMany(x => x.AssignedResources, assigned =>
        {
            assigned.ToTable("WorkItemAssignedResources");
            assigned.Property<long>("Id");
            assigned.HasKey("Id");

            assigned.WithOwner().HasForeignKey("WorkItemId");

            assigned.Property(a => a.ResourceBusinessKey)
                    .HasConversion<string>()
                    .HasMaxLength(200);

            assigned.Property(a => a.AllocatedHours);
            assigned.Property(a => a.AssignedDate);
            assigned.Property(a => a.RateType)
                    .HasConversion<string>()
                    .HasMaxLength(50);
        });

        builder.OwnsMany(x => x.Materials, mat =>
        {
            mat.ToTable("WorkItemMaterialUsage");
            mat.Property<long>("Id");
            mat.HasKey("Id");

            mat.WithOwner().HasForeignKey("WorkItemId");

            mat.Property(m => m.ResourceBusinessKey)
               .HasConversion<string>()
               .HasMaxLength(200);

            mat.Property(m => m.Quantity);
            mat.Property(m => m.UsageDate);
            mat.Property(m => m.Unit)
               .HasConversion<string>()
               .HasMaxLength(50);
        });

        builder.OwnsMany(x => x.Costs, cost =>
        {
            cost.ToTable("WorkItemCosts");
            cost.Property<long>("Id");
            cost.HasKey("Id");

            cost.WithOwner().HasForeignKey("WorkItemId");

            cost.Property(c => c.Type)
                .HasConversion<string>()
                .HasMaxLength(50);

            cost.Property(c => c.Amount);
            cost.Property(c => c.Currency).HasMaxLength(10);
            cost.Property(c => c.Description).HasMaxLength(500);
            cost.Property(c => c.CostDate);

            cost.Property(c => c.ReferenceNumber)
                .HasMaxLength(100);

            cost.Property(c => c.Notes)
                .HasMaxLength(500);

            cost.Property(c => c.Status)
                .HasConversion<string>()
                .HasMaxLength(50);

            cost.Property(c => c.BusinessKey)
                .HasConversion<string>()
                .HasMaxLength(200);
        });

        builder.OwnsMany(x => x.CheckLists, cl =>
        {
            cl.ToTable("WorkItemCheckLists");

            cl.Property<long>("Id");
            cl.HasKey("Id");

            cl.WithOwner().HasForeignKey("WorkItemId");

            cl.Property(c => c.Title)
              .HasMaxLength(300);

            cl.Property(c => c.BusinessKey)
              .HasConversion<string>()
              .HasMaxLength(200);

            cl.OwnsMany(c => c.Items, item =>
            {
                item.ToTable("WorkItemCheckListItems");

                item.Property<long>("Id");
                item.HasKey("Id");

                item.WithOwner().HasForeignKey("CheckListId");

                item.Property(i => i.Title).HasMaxLength(500);
                item.Property(i => i.IsCompleted);
            });
        });

        // -------------------------------
        // 🔹 OWNED: Work Logs
        // -------------------------------
        builder.OwnsMany(x => x.WorkItemLogWorks, log =>
        {
            log.ToTable("WorkItemLogWorks");

            log.Property<long>("Id");
            log.HasKey("Id");

            log.WithOwner().HasForeignKey("WorkItemId");

            log.Property(l => l.HoursSpent);
            log.Property(l => l.WorkDate);
            log.Property(l => l.Description).HasMaxLength(500);

            log.Property(l => l.ResourceBusinessKey)
               .HasConversion<string>()
               .HasMaxLength(200);

            log.Property(l => l.BusinessKey)
               .HasConversion<string>()
               .HasMaxLength(200);
        });

        // -------------------------------
        // 🔹 AutoInclude Navigations
        // -------------------------------
        builder.Navigation(x => x.CheckLists).AutoInclude();
        builder.Navigation(x => x.WorkItemLogWorks).AutoInclude();
        builder.Navigation(x => x.Costs).AutoInclude();
        builder.Navigation(x => x.AssignedResources).AutoInclude();
        builder.Navigation(x => x.Materials).AutoInclude();
    }
}
