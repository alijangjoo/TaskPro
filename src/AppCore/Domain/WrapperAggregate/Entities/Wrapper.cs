namespace Suspect.TaskPro.AppCore.Domain.WrapperAggregate.Entities;

using OysterFx.AppCore.Domain.Aggregates;
using OysterFx.AppCore.Domain.Exceptions;
using Suspect.TaskPro.AppCore.Domain.WrapperAggregate.Enums;

public class Wrapper : AggregateRoot
{
    public string? Title { get; private set; }
    public WrapperType Type { get; private set; }
    public LifeCycleState LifeCycleState { get; private set; }
    public long? ParentId { get; private set; }
    public long ProgressPercent { get; private set; }
    public int Weight { get; private set; }
    public string? Description { get; private set; }

    private readonly List<WrapperKpi> _wrapperKpis = new();
    public IReadOnlyCollection<WrapperKpi> WrapperKpis => _wrapperKpis.AsReadOnly();

    private Wrapper() { }

    private Wrapper(
        string? title,
        WrapperType type,
        long? parentId,
        long progressPercent,
        int weight,
        string? description)
    {
        Title = title;
        Type = type;
        ParentId = parentId;
        ProgressPercent = progressPercent;
        Weight = weight;
        Description = description;
        LifeCycleState = LifeCycleState.Unknown;
    }

    public static Wrapper CreateInstance(
        string? title,
        WrapperType type,
        long? parentId,
        long progressPercent,
        int weight,
        string? description)
        => new(title, type, parentId, progressPercent, weight, description);

    public void AddKpi(WrapperKpi wrapperKpi)
    {
        if (!_wrapperKpis.Contains(wrapperKpi))
            _wrapperKpis.Add(wrapperKpi);
    }

    public void SetParent(long? parentId) => ParentId = parentId;

    public void SetProgress(long progress)
    {
        if (progress < 0 || progress > 100)
            throw new AggregateStateExceptions("Progress must be between 0 and 100", "");

        ProgressPercent = progress;
    }

    public void SetState(LifeCycleState state)
    {
        if (!Enum.IsDefined(typeof(LifeCycleState), state))
            throw new AggregateStateExceptions("Invalid lifecycle", "");

        LifeCycleState = state;
    }
}
