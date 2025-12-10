namespace Suspect.TaskPro.AppCore.AppServices.Tasks.Costs.Commands.AddWorkItemCost;

using FluentValidation;
using Suspect.TaskPro.AppCore.Shared.Tasks.Costs.Commands.AddWorkItemCost;

public class AddWorkItemCostCommandValidator : AbstractValidator<AddWorkItemCostCommand>
{
    public AddWorkItemCostCommandValidator()
    {
    }
}