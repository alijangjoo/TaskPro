namespace Suspect.TaskPro.AppCore.Shared.Tasks.Estimates.CreateRevisionEstimate;

using OysterFx.AppCore.Shared.Commands;

public class RevisionEstimateCommand : ICommand<Guid>
{
    public Guid WorkItemBusinessKey { get; set; }
    public double? OptimisticHours { get; set; }
    public double? MostLikelyHours { get; set; }
    public double? PessimisticHours { get; set; }
    public double? ConfidenceLevel { get; set; }
    public string? Assumptions { get; set; }
    public Guid? RevisedByResourceBusinessKey { get; set; }
}