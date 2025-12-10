namespace Suspect.TaskPro.AppCore.AppServices.Tasks.LogWorks.Commands.AddWorkLog;

using FluentValidation;
using Suspect.TaskPro.AppCore.Shared.Tasks.LogWorks.Commands.AddWorkLog;

public class AddWorkLogCommandValidator : AbstractValidator<AddWorkLogCommand>
{
    public AddWorkLogCommandValidator()
    {        
    }
}