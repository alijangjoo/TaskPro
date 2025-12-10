namespace Suspect.TaskPro.AppCore.AppServices.WrapperAgg.Wrappers.Commands.CreateWrapper;

using OysterFx.AppCore.AppServices.Commands;
using OysterFx.AppCore.Shared.Commands.Common;
using Suspect.TaskPro.AppCore.Domain.WrapperAggregate.Entities;
using Suspect.TaskPro.AppCore.Shared.WrapperAgg.Wrappers.Commands;
using Suspect.TaskPro.AppCore.Shared.WrapperAgg.Wrappers.Commands.CreateWrapper;
using System;
using System.Threading.Tasks;

public class CreateWrapperCommandHandler : CommandHandler<CreateWrapperCommand, Guid>
{
    private readonly IWrapperCommandRepository _wrapperCommandRepository;

    public CreateWrapperCommandHandler(IWrapperCommandRepository wrapperCommandRepository)
    {
        _wrapperCommandRepository = wrapperCommandRepository;
    }
    public override async Task<CommandResult<Guid>> Handle(CreateWrapperCommand command)
    {
        var wrapper = Wrapper.CreateInstance(
            command.Title,
            command.Type,
            command.ParentId,
            command.ProgressPercent,
            command.Weight,
            command.Description);

        await _wrapperCommandRepository.InsertAsync(wrapper);
        await _wrapperCommandRepository.CommitAsync();
        return await OkAsync(wrapper.BusinessKey.Value);
    }
}