namespace Suspect.TaskPro.AppCore.AppServices.Tasks.LogWorks.Commands.RemoveWorkLog;

using FluentValidation;
using Suspect.TaskPro.AppCore.Shared.Tasks.LogWorks.Commands.RemoveWorkLog;

public class RemoveWorkLogCommandValidator : AbstractValidator<RemoveWorkLogCommand>
{
    public RemoveWorkLogCommandValidator()
    {        
    }
}