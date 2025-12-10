namespace Suspect.TaskPro.Infra.Persistence.Postgres.Queries.Tasks.Entities;

using Suspect.TaskPro.AppCore.Domain.TaskAggregate.Enums;

public class WorkItem
{
    public long Id { get; set; }
    public Guid BusinessKey { get; set; }
    public string? Title { get; set; }
    public bool HasAlarm { get; set; }
    public DateTime? AlarmTime { get; set; }
    public string? Description { get; set; } = string.Empty;
    public RecurrenceType RecurrenceType { get; set; }
    public WorkItemStatus Status { get; set; }
    public int Weight { get; set; }
    public long? ParentId { get; set; }
    public string? CreatedByUserId { get; set; }
}