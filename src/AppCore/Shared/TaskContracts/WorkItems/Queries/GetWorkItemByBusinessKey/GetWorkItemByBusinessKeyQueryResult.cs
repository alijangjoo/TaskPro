namespace Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Queries.GetWorkItemByBusinessKey;

using Suspect.TaskPro.AppCore.Domain.TaskAggregate.Enums;
using Suspect.TaskPro.AppCore.Shared.TaskContracts.WorkItems.Queries.Common;
using System;

public class GetWorkItemByBusinessKeyQueryResult
{
    public long Id { get; set; }
    public Guid BusinessKey { get; set; }
    public RecurrenceType RecurrenceType { get; set; }
    public WorkItemStatus Status { get; set; }
    public string? Title { get; set; }
    public DateTime? AlarmTime { get; set; }
    public bool HasAlarm { get; set; }
    public List<WorkItemScheduleDto> Schedules { get; set; } = new();
}