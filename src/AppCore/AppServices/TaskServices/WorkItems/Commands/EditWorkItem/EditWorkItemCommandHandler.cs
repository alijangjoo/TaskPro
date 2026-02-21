namespace Suspect.TaskPro.AppCore.AppServices.TaskServices.WorkItems.Commands.EditWorkItem;

using OysterFx.AppCore.AppServices.Commands;
using OysterFx.AppCore.Shared.Commands.Common;
using Suspect.TaskPro.AppCore.Shared.TaskContracts.WorkItems.Commands.EditWorkItem;
using Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Commands;
using System;
using System.Threading.Tasks;

public class EditWorkItemCommandHandler : CommandHandler<EditWorkItemCommand, Guid>
{
    private readonly IWorkItemCommandRepository _workItemCommandRepository;

    public EditWorkItemCommandHandler(IWorkItemCommandRepository workItemCommandRepository)
    {
        _workItemCommandRepository = workItemCommandRepository;
    }

    public override async Task<CommandResult<Guid>> Handle(EditWorkItemCommand command)
    {
        var workItem = await _workItemCommandRepository.GetGraphAsync(command.BusinessKey);
        workItem.Edit(command.Title, command.RecurrenceType, command.ScheduleDates);
        await _workItemCommandRepository.CommitAsync();
        return await OkAsync(workItem.BusinessKey.Value);
    }
}