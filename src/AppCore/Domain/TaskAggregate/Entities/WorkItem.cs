namespace Suspect.TaskPro.AppCore.Domain.TaskAggregate.Entities;

using OysterFx.AppCore.Domain.Aggregates;
using OysterFx.AppCore.Domain.Exceptions;
using OysterFx.AppCore.Domain.ValueObjects;
using Suspect.TaskPro.AppCore.Domain.ResourcesAggregate.Entities;
using Suspect.TaskPro.AppCore.Domain.ResourcesAggregate.Enums;
using Suspect.TaskPro.AppCore.Domain.TaskAggregate.Enums;

public class WorkItem : AggregateRoot
{
    public string? Title { get; private set; }
    public string? Description { get; private set; } = string.Empty;
    public RecurrenceType RecurrenceType { get; private set; }
    public WorkItemStatus Status { get; private set; }
    public int Weight { get; private set; }
    public long? ParentId { get; private set; }
    public bool HasAlarm { get; set; }
    public DateTime? AlarmTime { get; set; }

    public WorkItemEstimate? Estimate { get; private set; }

    private readonly List<WorkItemResourceAssignment> _assignedResources = new();
    public IReadOnlyCollection<WorkItemResourceAssignment> AssignedResources => _assignedResources.AsReadOnly();


    private readonly List<WorkItemCost> _costs = new();
    public IReadOnlyCollection<WorkItemCost> Costs => _costs.AsReadOnly();


    private readonly List<WorkItemResourceMaterialUsage> _materials = new();
    public IReadOnlyCollection<WorkItemResourceMaterialUsage> Materials => _materials.AsReadOnly();


    private readonly List<WorkItemEstimate> _estimateHistory = new();
    public IReadOnlyCollection<WorkItemEstimate> EstimateHistory => _estimateHistory.AsReadOnly();


    private readonly List<WorkItemCheckList> _checkLists = new();
    public IReadOnlyCollection<WorkItemCheckList> CheckLists => _checkLists.AsReadOnly();


    private readonly List<WorkItemLogWork> _workItemLogWorks = new();
    public IReadOnlyCollection<WorkItemLogWork> WorkItemLogWorks => _workItemLogWorks.AsReadOnly();


    private readonly List<WorkItemSchedule> _schedules = new();
    public IReadOnlyCollection<WorkItemSchedule> Schedules => _schedules.AsReadOnly();

    protected WorkItem() { }

    protected WorkItem(
        string? title,
        RecurrenceType recurrenceType,
        List<DateTime>? scheduleDates = null)
    {
        Status = WorkItemStatus.Unknown;
        Title = title;
        RecurrenceType = recurrenceType;
        HasAlarm = false;

        if (scheduleDates != null && scheduleDates.Any())
            AddRecurringSchedule(recurrenceType, scheduleDates);
    }

    public static WorkItem CreateInstance(
        string? title,
        RecurrenceType recurrenceType,
        List<DateTime>? scheduleDates = null)
        => new(title, recurrenceType, scheduleDates);


    public void SetAlarm(DateTime alarmTime)
    {
        HasAlarm = true;
        AlarmTime = alarmTime;
    }

    public void ClearAlarm()
    {
        HasAlarm = false;
        AlarmTime = null;
    }
    public void AddSchedule(DateTime scheduledDate)
    {
        if (_schedules.Any(s => s.ScheduledDate.Date == scheduledDate.Date))
            throw new InvalidOperationException("This date is already scheduled.");

        _schedules.Add(WorkItemSchedule.CreateSchedule(BusinessKey, scheduledDate));
    }

    public void AddRecurringSchedule(RecurrenceType recurrenceType, List<DateTime> scheduleDates)
    {
        foreach (var date in scheduleDates)
        {
            if (date.Date < DateTime.UtcNow.Date)
                continue;

            AddSchedule(date);
        }
    }

    public void MarkScheduleAsDone(BusinessKey scheculeBusinessKey)
    {
        var schedule = _schedules.FirstOrDefault(s => s.BusinessKey == scheculeBusinessKey);
        if (schedule is not null)
        {
            schedule.MarkAsDone();
            if (_schedules.Count(c => c.IsDone) == _schedules.Count)
                Status = WorkItemStatus.Done;
        }

        else throw new AggregateStateExceptions("تاریخ ارسالی برای تسک تعریف نشده است", "");
    }

    public WorkItemResourceAssignment AssignResource(
        BusinessKey resourceKey,
        decimal allocatedHours,
        RateType rateType,
        DateTime assignedDate)
    {
        var assignment = WorkItemResourceAssignment.CreateInstance(
            BusinessKey,
            resourceKey,
            allocatedHours,
            assignedDate,
            rateType
        );

        _assignedResources.Add(assignment);
        return assignment;
    }

    public void UnassignResource(BusinessKey resourceKey)
    {
        var resource = _assignedResources.FirstOrDefault(x => x.ResourceBusinessKey == resourceKey);
        if (resource is not null)
            _assignedResources.Remove(resource);
    }

    public IEnumerable<WorkItemResourceAssignment> GetAssignedResources()
        => _assignedResources.AsEnumerable();

    /// <summary>
    /// Calculate resource cost based on hours, date, and rateType
    /// </summary>
    public decimal GetTotalResourceCost(DateTime date, Func<BusinessKey, Resource?> resourceResolver)
    {
        decimal total = 0;

        foreach (var assignment in _assignedResources)
        {
            var resource = resourceResolver(assignment.ResourceBusinessKey);
            if (resource is null) continue;

            var rate = resource.GetRate(date, assignment.RateType);
            if (rate is not null)
                total += assignment.AllocatedHours * rate.HourlyRate;
            else if (resource.CostPerHour != null)
                total += assignment.AllocatedHours * resource.CostPerHour.Value;
        }

        return total;
    }

    public WorkItemResourceMaterialUsage AddMaterialUsage(
        BusinessKey resourceKey,
        decimal quantity,
        DateTime usageDate,
        MaterialUnit materialUnit)
    {
        var material = WorkItemResourceMaterialUsage.CreateInstance(
            BusinessKey,
            resourceKey,
            quantity,
            usageDate,
            materialUnit
        );

        _materials.Add(material);
        return material;
    }

    public decimal GetTotalMaterialCost(DateTime date, Func<BusinessKey, Resource?> resourceResolver)
    {
        decimal total = 0;

        foreach (var usage in Materials)
        {
            var resource = resourceResolver(usage.ResourceBusinessKey);
            if (resource is null) continue;

            if (resource.Type != ResourceType.Material)
                continue;

            var rate = resource.GetRate(date, RateType.Standard);

            if (rate is not null)
                total += usage.Quantity * rate.HourlyRate;
        }

        return total;
    }

    public double TotalHoursSpent => _workItemLogWorks.Sum(x => x.HoursSpent);
    public double ProgressPercent => CalculateProgress();

    private double CalculateProgress()
    {
        if (CheckLists.Any())
        {
            var totalItems = CheckLists.SelectMany(cl => cl.Items).Count();
            var completedItems = CheckLists.SelectMany(cl => cl.Items).Count(item => item.IsCompleted);
            return totalItems > 0 ? completedItems / (double)totalItems * 100 : 0;
        }
        return 0;
    }

    public void SetInitialEstimate(
        double optimisticHours,
        double mostLikelyHours,
        double pessimisticHours,
        double confidenceLevel,
        BusinessKey estimatedByResourceBusinessKey,
        string? assumptions = null)
    {
        var estimate = WorkItemEstimate.CreateInitial(
            optimisticHours,
            mostLikelyHours,
            pessimisticHours,
            confidenceLevel,
            estimatedByResourceBusinessKey!,
            assumptions);
        Estimate = estimate;
    }
    public void ReviseEstimate(WorkItemEstimate newEstimate)
    {
        if (newEstimate.Type != EstimateType.Revised)
            throw new ArgumentException("Estimate type must be Revised");

        if (Estimate is not null)
            _estimateHistory.Add(Estimate);

        Estimate = newEstimate;
    }
    public double? GetEstimationAccuracy()
    {
        if (Estimate is not null || TotalHoursSpent == 0)
            return null;
        var variance = Math.Abs(Estimate.ExpectedHours - TotalHoursSpent);
        return (1 - variance / Estimate.ExpectedHours) * 100;
    }
    public void ReviseEstimate(
        double? optimisticHours = null,
        double? mostLikelyHours = null,
        double? pessimisticHours = null,
        double? confidenceLevel = null,
        string? assumptions = null,
        Guid? revisedByResourceBusinessKey = null)
    {
        if (Estimate is null)
            throw new InvalidOperationException("No existing estimate to revise");

        _estimateHistory.Add(Estimate);

        Estimate = Estimate.CreateRevision(
            optimisticHours,
            mostLikelyHours,
            pessimisticHours,
            confidenceLevel,
            assumptions,
            revisedByResourceBusinessKey!);
    }
    public decimal TotalPlannedCost
        => Costs.Where(c => c.Status == CostStatus.Planned || c.Status == CostStatus.Approved).Sum(c => c.Amount);

    public decimal TotalActualCost
        => Costs.Where(c => c.Status == CostStatus.Paid).Sum(c => c.Amount);

    public decimal TotalInvoicedCost
        => Costs.Where(c => c.Status == CostStatus.Invoiced || c.Status == CostStatus.Paid).Sum(c => c.Amount);

    public decimal CostVariance => TotalActualCost - TotalPlannedCost;
    public decimal CostPerformanceIndex => TotalPlannedCost > 0 ? TotalPlannedCost / TotalActualCost : 0;

    public void AddCost(
        CostType type,
        decimal amount,
        string currency,
        string description,
        DateTime costDate,
        string? referenceNumber = null,
        string? notes = null)
    {
        var cost = WorkItemCost.CreateInstance(
            BusinessKey,
            type,
            amount,
            currency,
            description,
            costDate,
            referenceNumber,
            notes);
        _costs.Add(cost);
    }
    public void RemoveCost(BusinessKey costBusinessKey)
    {
        var cost = Costs.FirstOrDefault(c => c.BusinessKey == costBusinessKey);
        if (cost is not null)
            _costs.Remove(cost);
    }

    public WorkItemCost? GetCost(long costId)
        => Costs.FirstOrDefault(c => c.Id == costId);

    public IEnumerable<WorkItemCost> GetCostsByType(CostType type)
        => Costs.Where(c => c.Type == type);
    public IEnumerable<WorkItemCost> GetCostsByStatus(CostStatus status)
        => Costs.Where(c => c.Status == status);

    public decimal GetCostByType(CostType type)
        => Costs.Where(c => c.Type == type && (c.Status == CostStatus.Planned || c.Status == CostStatus.Approved)).Sum(c => c.Amount);

    public WorkItemCheckList AddCheckList(string title)
    {
        var checkList = WorkItemCheckList.CreateInstance(BusinessKey, title);
        _checkLists.Add(checkList);
        return checkList;
    }

    public void RemoveCheckList(BusinessKey ckeckListBusinessKey)
    {
        var checkList = CheckLists.FirstOrDefault(x => x.BusinessKey == ckeckListBusinessKey);
        if (checkList is not null)
            _checkLists.Remove(checkList);
    }

    public WorkItemLogWork AddWorkLog(
        double hoursSpent,
        DateTime workDate,
        BusinessKey loggedByResourceBusinessKey,
        string? description = null)
    {
        var log = WorkItemLogWork.CreateInstance(
            BusinessKey,
            hoursSpent,
            workDate,
            loggedByResourceBusinessKey,
            description);
        _workItemLogWorks.Add(log);
        return log;
    }

    public void RemoveWorkLog(BusinessKey logWorkBusinessKey)
    {
        var log = _workItemLogWorks.FirstOrDefault(x => x.BusinessKey == logWorkBusinessKey);
        if (log is not null)
            _workItemLogWorks.Remove(log);
    }

    public void Edit(string? title, RecurrenceType recurrenceType, List<DateTime>? scheduleDates = null)
    {
        if (Status == WorkItemStatus.Done)
            throw new InvalidOperationException("Cannot edit a completed work item");

        if (title != null)
            Title = title;

        if (recurrenceType != RecurrenceType)
            RecurrenceType = recurrenceType;

        if (scheduleDates != null && scheduleDates.Any())
        {
            var futureSchedules = _schedules
                .Where(s => s.ScheduledDate.Date >= DateTime.UtcNow.Date)
                .ToList();

            foreach (var schedule in futureSchedules)
            {
                if (!scheduleDates.Any(d => d.Date == schedule.ScheduledDate.Date))
                    _schedules.Remove(schedule);
            }

            foreach (var date in scheduleDates)
            {
                if (date.Date < DateTime.UtcNow.Date)
                    continue;

                if (!_schedules.Any(s => s.ScheduledDate.Date == date.Date))
                    AddSchedule(date);
            }
        }
    }
}