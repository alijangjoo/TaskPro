namespace Suspect.TaskPro.AppCore.Domain.TaskOrchestration.Entities;

using Suspect.TaskPro.AppCore.Domain.TaskOrchestration.Enums;

public class WorkFlow
{
    public long SourseTaskId { get; set; }
    public long TargetTaskId { get; set; }
    public DependancyType DependancyType { get; set; }
    public int LogTime { get; set; }
    public int TriggerCondition { get; set; } // Example: Progress >= 70%
}