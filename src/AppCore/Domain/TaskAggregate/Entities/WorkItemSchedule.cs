namespace Suspect.TaskPro.AppCore.Domain.TaskAggregate.Entities;

using OysterFx.AppCore.Domain.Aggregates;
using OysterFx.AppCore.Domain.ValueObjects;
using System;

public class WorkItemSchedule : Aggregate
{
    public BusinessKey WorkItemBusinessKey { get; private set; }
    public DateTime ScheduledDate { get; private set; }
    public bool IsDone { get; private set; }

    protected WorkItemSchedule() { }

    private WorkItemSchedule(BusinessKey workItemBusinessKey, DateTime scheduledDate)
    {
        ScheduledDate = scheduledDate;
        IsDone = false;
        WorkItemBusinessKey = workItemBusinessKey;
    }

    public static WorkItemSchedule CreateSchedule(BusinessKey workItemBusinessKey, DateTime scheduledDate) 
        => new(workItemBusinessKey, scheduledDate);

    public void MarkAsDone()
    {
        IsDone = true;
    }

    public void MarkAsUndone()
    {
        IsDone = false;
    }
}