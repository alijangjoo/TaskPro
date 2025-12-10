namespace Suspect.TaskPro.AppCore.Domain.TaskAggregate.Entities;

using OysterFx.AppCore.Domain.Aggregates;
using OysterFx.AppCore.Domain.ValueObjects;
using Suspect.TaskPro.AppCore.Domain.ResourcesAggregate.Enums;

public class WorkItemResourceMaterialUsage : Aggregate
{
    public BusinessKey WorkItemBusinessKey { get; private set; }
    public BusinessKey ResourceBusinessKey { get; private set; }
    public decimal Quantity { get; private set; }
    public DateTime UsageDate { get; private set; }
    public MaterialUnit Unit { get; private set; }
    private WorkItemResourceMaterialUsage() { }

    private WorkItemResourceMaterialUsage(
        BusinessKey workItemKey,
        BusinessKey resourceKey,
        decimal quantity,
        DateTime usageDate,
        MaterialUnit unit)
    {
        WorkItemBusinessKey = workItemKey;
        ResourceBusinessKey = resourceKey;
        Quantity = quantity;
        UsageDate = usageDate;
        Unit = unit;
    }

    public static WorkItemResourceMaterialUsage CreateInstance(
        BusinessKey workItemKey,
        BusinessKey resourceKey,
        decimal quantity,
        DateTime usageDate,
        MaterialUnit unit)
        => new(workItemKey, resourceKey, quantity, usageDate, unit);
}
