namespace Suspect.TaskPro.AppCore.Shared.TaskContracts.WorkItems.Commands.DoneWorkItem;

using OysterFx.AppCore.Shared.Commands;
using System;

public class DoneWorkItemCommand : ICommand<bool>
{
    public Guid WorkItemBusinessKey { get; set; }
    public Guid ScheduleBusinessKey { get; set; }
    public DoneWorkItemCommand(Guid workItemBusinessKey, Guid scheduleBusinessKey)
    {
        ScheduleBusinessKey = scheduleBusinessKey;
        WorkItemBusinessKey = workItemBusinessKey;
    }
    public static DoneWorkItemCommand CreateInstance(Guid workItemBusinessKey, Guid scheduleBusinessKeys)
        => new DoneWorkItemCommand(workItemBusinessKey, scheduleBusinessKeys);
}