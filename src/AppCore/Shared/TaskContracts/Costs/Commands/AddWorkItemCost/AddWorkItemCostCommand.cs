namespace Suspect.TaskPro.AppCore.Shared.Tasks.Costs.Commands.AddWorkItemCost;

using OysterFx.AppCore.Shared.Commands;
using Suspect.TaskPro.AppCore.Domain.TaskAggregate.Enums;
using System;

public class AddWorkItemCostCommand : ICommand<Guid>
{
    public Guid WorkItemBusinessKey { get; set; }
    public CostType Type { get; set; }
    public decimal Amount { get; set; }
    public string? Currency { get; set; }
    public string? Description { get; set; }
    public DateTime CostDate { get; set; }
    public string? ReferenceNumber { get; set; }
    public string? Notes { get; set; }
}