namespace Suspect.TaskPro.AppCore.Shared.Tasks.Costs.Commands.RemoveCost;

using OysterFx.AppCore.Shared.Commands;
using System;

public class RemoveCostCommand : ICommand<bool>
{
    public Guid WorkItemBusinessGuid { get; set; }
    public Guid CostBusinessKey { get; set; }
}