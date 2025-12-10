namespace Suspect.TaskPro.AppCore.AppServices.Tasks.Estimates.Commands.RevisionEstimate;

using FluentValidation;
using Suspect.TaskPro.AppCore.Shared.Tasks.Estimates.CreateRevisionEstimate;

public class RevisionEstimateCommandValidator : AbstractValidator<RevisionEstimateCommand>
{
    public RevisionEstimateCommandValidator()
    {        
    }
}