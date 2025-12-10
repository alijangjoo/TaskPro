namespace Suspect.TaskPro.AppCore.Domain.TaskAggregate.Entities;

using OysterFx.AppCore.Domain.Aggregates;
using OysterFx.AppCore.Domain.ValueObjects;
using Suspect.TaskPro.AppCore.Domain.ResourcesAggregate.Enums;

public class WorkItemResourceAssignment : Aggregate
{
    public BusinessKey WorkItemBusinessKey { get; private set; }
    public BusinessKey ResourceBusinessKey { get; private set; }
    public decimal AllocatedHours { get; private set; }
    public DateTime AssignedDate { get; private set; }
    public RateType RateType { get; private set; }
    public ResourceAssignmentStatus Status { get; private set; }
    private WorkItemResourceAssignment() { }

    private WorkItemResourceAssignment(
        BusinessKey workItemKey,
        BusinessKey resourceKey,
        decimal allocatedHours,
        DateTime assignedDate,
        RateType rateType)
    {
        WorkItemBusinessKey = workItemKey;
        ResourceBusinessKey = resourceKey;
        AllocatedHours = allocatedHours;
        AssignedDate = assignedDate;
        RateType = rateType;
    }

    public static WorkItemResourceAssignment CreateInstance(
        BusinessKey workItemKey,
        BusinessKey resourceKey,
        decimal allocatedHours,
        DateTime assignedDate,
        RateType rateType)
        => new(workItemKey, resourceKey, allocatedHours, assignedDate, rateType);
}
