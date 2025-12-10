namespace Suspect.TaskPro.AppCore.Shared.TaskContracts.WorkItems.Queries.Common;

using System;

public class WorkItemScheduleDto
{
    public long Id { get; set; }
    public Guid BusinessKey { get; set; }
    public Guid WorkItemBusinessKey { get; set; }
    public DateTime ScheduledDate { get; set; }
    public bool IsDone { get; set; }
}