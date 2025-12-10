namespace Suspect.TaskPro.AppCore.AppServices.Tasks.CheckLists.Commands.AddCheckList;

using FluentValidation;
using Suspect.TaskPro.AppCore.Shared.Tasks.CheckLists.Commands.AddCheckList;

public class AddCheckListCommandValidator : AbstractValidator<AddCheckListCommand>
{
    public AddCheckListCommandValidator()
    {
    }
}