namespace Suspect.TaskPro.AppCore.Domain.TaskAggregate.Entities;

using OysterFx.AppCore.Domain.Aggregates;
using OysterFx.AppCore.Domain.ValueObjects;

public class WorkItemCheckList : Aggregate
{
    public BusinessKey WorkItemBusinessKey { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public DateTime CreatedDate { get; private set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; private set; }
    public bool IsDeleted { get; private set; } = false;

    public ICollection<WorkItemCheckListItem> Items { get; set; } = new List<WorkItemCheckListItem>();

    private WorkItemCheckList()
    {
    }
    private WorkItemCheckList(BusinessKey workItemBusinessKey, string title)
    {
        WorkItemBusinessKey = workItemBusinessKey;
        Title = title;
    }
    public static WorkItemCheckList CreateInstance(BusinessKey workItemBusinessKey, string title)
        => new(workItemBusinessKey, title);

    public WorkItemCheckListItem AddItem(string title, string? description = null)
    {
        var item = new WorkItemCheckListItem(BusinessKey, title, description);
        Items.Add(item);
        return item;
    }

    public void RemoveItem(long itemId)
    {
        var item = Items.FirstOrDefault(x => x.Id == itemId);
        if (item != null)
            Items.Remove(item);
    }

    public void MarkItemCompleted(long itemId)
    {
        var item = Items.FirstOrDefault(x => x.Id == itemId);
        item?.MarkCompleted();
    }

    public void MarkItemUncompleted(long itemId)
    {
        var item = Items.FirstOrDefault(x => x.Id == itemId);
        item?.MarkUncompleted();
    }
}