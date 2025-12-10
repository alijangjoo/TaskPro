namespace Suspect.TaskPro.AppCore.Domain.TaskAggregate.Entities;

using OysterFx.AppCore.Domain.Aggregates;
using OysterFx.AppCore.Domain.ValueObjects;

public class WorkItemCheckListItem : Aggregate
{
    public BusinessKey WorkItemCheckListBusinessKey { get; private set; }
    public BusinessKey CompletedByResourceBusinessKey { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public bool IsCompleted { get; private set; } = false;
    public DateTime? CompletedDate { get; private set; }
    public int Order { get; private set; }

    private WorkItemCheckListItem() { }

    public WorkItemCheckListItem(BusinessKey workItemCheckListBusinessKey, string title, string? description = null)
    {
        WorkItemCheckListBusinessKey = workItemCheckListBusinessKey;
        Title = title;
        Description = description;
    }

    public void MarkCompleted()
    {
        IsCompleted = true;
        CompletedDate = DateTime.UtcNow;
    }

    public void MarkUncompleted()
    {
        IsCompleted = false;
        CompletedDate = null;
    }

    public void Update(string title, string? description = null)
    {
        Title = title;
        Description = description;
    }
}