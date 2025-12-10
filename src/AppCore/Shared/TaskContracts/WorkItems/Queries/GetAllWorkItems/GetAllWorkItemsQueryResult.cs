namespace Suspect.TaskPro.AppCore.Shared.TaskContracts.WorkItems.Queries.GetAllWorkItems;

using Suspect.TaskPro.AppCore.Domain.TaskAggregate.Enums;
using Suspect.TaskPro.AppCore.Shared.TaskContracts.WorkItems.Queries.Common;
using System;
using System.Collections.Generic;

public class GetAllWorkItemsQueryResult
{
    public long Id { get; set; }
    public Guid BusinessKey { get; set; }
    public RecurrenceType RecurrenceType { get; set; }
    public WorkItemStatus Status { get; set; }
    public string? Title { get; set; }
    public bool HasAlarm { get; set; }
    public DateTime? AlarmTime { get; set; }
    public List<WorkItemScheduleDto> Schedules { get; set; } = new();
}