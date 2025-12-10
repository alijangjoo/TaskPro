namespace Suspect.TaskPro.AppCore.AppServices.Tasks.CheckLists.Commands.RemoveCheckList;

using FluentValidation;
using Suspect.TaskPro.AppCore.Shared.Tasks.CheckLists.Commands.RemoveCheckList;

public class RemoveCheckListCommandValidator : AbstractValidator<RemoveCheckListCommand>
{
    public RemoveCheckListCommandValidator()
    {
    }
}