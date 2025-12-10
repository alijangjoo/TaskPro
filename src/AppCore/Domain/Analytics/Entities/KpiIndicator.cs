namespace Suspect.TaskPro.AppCore.Domain.Analytics.Entities;

public class KpiIndicator
{
    public string? Name { get; set; } // Productivity, FinancialHealth, etc
    public string? Expression { get; set; } // ("(Sum(Task.Progress * Task.Weight)) / Count(Task.Id)")
    public int TargetValue { get; set; }
}