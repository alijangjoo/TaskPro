namespace Suspect.TaskPro.AppCore.Domain.TaskAggregate.Entities;

using OysterFx.AppCore.Domain.Aggregates;
using OysterFx.AppCore.Domain.Exceptions;
using OysterFx.AppCore.Domain.ValueObjects;
using Suspect.TaskPro.AppCore.Domain.TaskAggregate.Enums;

public class WorkItemLogWork : Aggregate
{
    public BusinessKey WorkItemBusinessKey { get; private set; }
    public BusinessKey ResourceBusinessKey { get; private set; } // کاربری که کار را انجام داده
    public string Description { get; private set; } = string.Empty;
    public double HoursSpent { get; private set; } // تعداد ساعت‌های صرف شده
    public DateTime WorkDate { get; private set; } // تاریخ انجام کار
    public DateTime LogDate { get; private set; } = DateTime.UtcNow; // تاریخ ثبت لاگ
    public WorkLogType LogType { get; private set; } // نوع فعالیت
    public string? Notes { get; private set; } // یادداشت‌های اضافی

    private WorkItemLogWork() { }
    private WorkItemLogWork(
        BusinessKey workItemBusinessKey,
        double hoursSpent,
        DateTime workDate,
        BusinessKey resourceBusinessKey,
        string? description = null)
    {
        if (hoursSpent <= 0)
            throw new AggregateStateExceptions("Hours spent must be greater than zero.", "");

        WorkItemBusinessKey = workItemBusinessKey;
        ResourceBusinessKey = resourceBusinessKey;
        HoursSpent = hoursSpent;
        WorkDate = workDate;
        Description = description!;
    }

    public static WorkItemLogWork CreateInstance(
      BusinessKey workItemBusinessKey,
      double hoursSpent,
      DateTime workDate,
      BusinessKey resourceBusinessKey,
      string? description = null) => new(workItemBusinessKey, hoursSpent, workDate, resourceBusinessKey, description);

    public void Update(double? hoursSpent = null, string? description = null, DateTime? workDate = null)
    {
        if (hoursSpent.HasValue && hoursSpent.Value <= 0)
            throw new ArgumentException("Hours spent must be greater than zero.");

        HoursSpent = hoursSpent ?? HoursSpent;
        Description = description ?? Description;
        WorkDate = workDate ?? WorkDate;
    }
}