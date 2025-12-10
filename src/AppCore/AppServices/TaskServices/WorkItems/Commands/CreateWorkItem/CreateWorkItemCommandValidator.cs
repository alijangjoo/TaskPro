namespace Suspect.TaskPro.AppCore.AppServices.Tasks.WorkItems.Commands.CreateWorkItem;

using FluentValidation;
using Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Commands.CreateWorkItem;

public class CreateWorkItemCommandValidator : AbstractValidator<CreateWorkItemCommand>
{
    public CreateWorkItemCommandValidator()
    {
    }
}