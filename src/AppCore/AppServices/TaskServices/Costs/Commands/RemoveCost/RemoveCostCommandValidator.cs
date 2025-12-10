namespace Suspect.TaskPro.AppCore.AppServices.Tasks.Costs.Commands.RemoveCost;

using FluentValidation;
using Suspect.TaskPro.AppCore.Shared.Tasks.Costs.Commands.RemoveCost;

public class RemoveCostCommandValidator : AbstractValidator<RemoveCostCommand>
{
    public RemoveCostCommandValidator()
    {
    }
}