namespace Suspect.TaskPro.AppCore.AppServices.WrapperAgg.Wrappers.Commands.CreateWrapper;

using FluentValidation;
using Suspect.TaskPro.AppCore.Shared.WrapperAgg.Wrappers.Commands.CreateWrapper;

public class CreateWrapperCommandValidator : AbstractValidator<CreateWrapperCommand>
{
    public CreateWrapperCommandValidator()
    {
    }
}
