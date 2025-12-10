namespace Suspect.TaskPro.AppCore.Domain.ResourcesAggregate.Entities;

using OysterFx.AppCore.Domain.Aggregates;
using Suspect.TaskPro.AppCore.Domain.ResourcesAggregate.Enums;

public class Resource : AggregateRoot
{
    public string Name { get; private set; }
    public ResourceType Type { get; private set; }
    public decimal? CostPerHour { get; private set; }
    public string? AvailabilitySchedule { get; private set; }   // JSON like: {"Mon":"8-16","Tue":"8-16"}
    public List<string> Skills { get; private set; } = new();
    public int CapacityPerDay { get; private set; } = 8;
    public bool IsActive { get; private set; } = true;

    private readonly List<ResourceRate> _rates = new();
    public IReadOnlyCollection<ResourceRate> Rates => _rates.AsReadOnly();

    private Resource() { }

    private Resource(
        string name,
        ResourceType type,
        decimal? baseCost,
        string? schedule,
        IEnumerable<string>? skillTags,
        int capacityPerDay)
    {
        Name = name;
        Type = type;
        CostPerHour = baseCost;
        AvailabilitySchedule = schedule;
        CapacityPerDay = capacityPerDay;
        if (skillTags != null) Skills = skillTags.ToList();
    }

    public static Resource CreateInstance(
        string name,
        ResourceType type,
        decimal? baseCost,
        string? schedule,
        IEnumerable<string>? skills,
        int capacityPerDay = 8)
        => new(name, type, baseCost, schedule, skills, capacityPerDay);

    public void AddSkill(string skill)
    {
        if (!Skills.Contains(skill))
            Skills.Add(skill);
    }

    public void RemoveSkill(string skill)
    {
        Skills.Remove(skill);
    }

    public void AddRate(ResourceRate rate)
    {
        _rates.Add(rate);
    }

    public ResourceRate? GetRate(DateTime date, RateType type)
    {
        return _rates
            .Where(r => r.Type == type && r.IsActive(date))
            .OrderByDescending(r => r.EffectiveDate)
            .FirstOrDefault();
    }

    public decimal GetCostForDuration(decimal hours, DateTime date, RateType type = RateType.Standard)
    {
        var rate = GetRate(date, type);
        return rate != null ? hours * rate.HourlyRate : (CostPerHour ?? 0) * hours;
    }

    public bool IsAvailable(DateTime date)
    {
        if (!IsActive) return false;

        // simple example: checks if the weekday exists in schedule JSON
        if (AvailabilitySchedule == null) return true;

        // sophisticated availability parsing handled in application layer
        return true;
    }

    public void Deactivate() => IsActive = false;

    public void Activate() => IsActive = true;
}