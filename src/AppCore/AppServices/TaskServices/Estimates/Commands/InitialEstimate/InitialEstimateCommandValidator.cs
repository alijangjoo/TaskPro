namespace Suspect.TaskPro.AppCore.AppServices.Tasks.Estimates.Commands.InitialEstimate;

using FluentValidation;
using Suspect.TaskPro.AppCore.Shared.Tasks.Estimates.InitialEstimate;

public class InitialEstimateCommandValidator : AbstractValidator<InitialEstimateCommand>
{
    public InitialEstimateCommandValidator()
    {
    }
}