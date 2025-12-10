namespace Suspect.TaskPro.AppCore.Domain.TaskAggregate.Entities;

using OysterFx.AppCore.Domain.Aggregates;
using OysterFx.AppCore.Domain.ValueObjects;
using Suspect.TaskPro.AppCore.Domain.TaskAggregate.Enums;

public class WorkItemEstimate : Aggregate
{
    /// <summary>
    /// تخمین توسط چه شخصی ثبت شده است
    /// </summary>
    public BusinessKey EstimatedByResourceBusinessKey { get; private set; }

    /// <summary>
    /// خوش بینانه ترین زمان
    /// </summary>
    public double OptimisticHours { get; private set; }

    /// <summary>
    /// محتمل ترین زمان
    /// </summary>
    public double MostLikelyHours { get; private set; }

    /// <summary>
    /// بدبینانه ترین زمان
    /// </summary>
    public double PessimisticHours { get; private set; }

    /// <summary>
    /// میانگین وزنی سه تخمین قبلی (PERT)
    /// </summary>
    public double ExpectedHours => CalculateExpectedHours();

    /// <summary>
    /// سطح اطمینان به تخمین (درصد اطمینان از تخمین ارائه شده)
    /// </summary>
    public double ConfidenceLevel { get; private set; }

    /// <summary>
    /// تاریخ ثبت تخمین
    /// </summary>
    public DateTime EstimateDate { get; private set; }

    /// <summary>
    /// مفروضات (فرضیات و پیش نیازهایی که تخمین ها براساس آن ارائه شده است)
    /// </summary>
    public string? Assumptions { get; private set; }

    public EstimateType Type { get; private set; }

    private WorkItemEstimate() { }

    public WorkItemEstimate(
        double optimisticHours,
        double mostLikelyHours,
        double pessimisticHours,
        double confidenceLevel,
        BusinessKey estimatedByResourceBusinessKey,
        EstimateType type,
        string? assumptions = null)
    {
        ValidateHours(optimisticHours, mostLikelyHours, pessimisticHours);
        ValidateConfidenceLevel(confidenceLevel);

        OptimisticHours = optimisticHours;
        MostLikelyHours = mostLikelyHours;
        PessimisticHours = pessimisticHours;
        ConfidenceLevel = confidenceLevel;
        EstimatedByResourceBusinessKey = estimatedByResourceBusinessKey;
        Type = type;
        Assumptions = assumptions;
        EstimateDate = DateTime.UtcNow;
    }

    private double CalculateExpectedHours() => (OptimisticHours + 4 * MostLikelyHours + PessimisticHours) / 6;

    public WorkItemEstimate CreateRevision(
        double? optimisticHours = null,
        double? mostLikelyHours = null,
        double? pessimisticHours = null,
        double? confidenceLevel = null,
        string? assumptions = null,
        BusinessKey revisedByResourceBusinessKey = null)
    {
        return new WorkItemEstimate(
            optimisticHours ?? OptimisticHours,
            mostLikelyHours ?? MostLikelyHours,
            pessimisticHours ?? PessimisticHours,
            confidenceLevel ?? ConfidenceLevel,
            revisedByResourceBusinessKey ?? EstimatedByResourceBusinessKey,
            EstimateType.Revised,
            assumptions ?? Assumptions);
    }

    public static WorkItemEstimate CreateInitial(
        double optimisticHours,
        double mostLikelyHours,
        double pessimisticHours,
        double confidenceLevel,
        BusinessKey estimatedByResourceBusinessKey,
        string? assumptions = null)
    {
        return new WorkItemEstimate(
            optimisticHours,
            mostLikelyHours,
            pessimisticHours,
            confidenceLevel,
            estimatedByResourceBusinessKey,
            EstimateType.Initial,
            assumptions);
    }

    private static void ValidateHours(double optimistic, double mostLikely, double pessimistic)
    {
        if (optimistic <= 0 || mostLikely <= 0 || pessimistic <= 0)
            throw new ArgumentException("All hour estimates must be positive");

        if (optimistic > mostLikely || mostLikely > pessimistic)
            throw new ArgumentException("Hours must follow: Optimistic <= MostLikely <= Pessimistic");
    }

    private static void ValidateConfidenceLevel(double confidenceLevel)
    {
        if (confidenceLevel < 0 || confidenceLevel > 100)
            throw new ArgumentException("Confidence level must be between 0 and 100");
    }

    public override bool Equals(object? obj)
    {
        return obj is WorkItemEstimate other &&
               OptimisticHours == other.OptimisticHours &&
               MostLikelyHours == other.MostLikelyHours &&
               PessimisticHours == other.PessimisticHours &&
               ConfidenceLevel == other.ConfidenceLevel &&
               Type == other.Type;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(OptimisticHours, MostLikelyHours, PessimisticHours, ConfidenceLevel, Type);
    }
}