namespace Suspect.TaskPro.AppCore.AppServices.Tasks.LogWorks.Commands.AddWorkLog;

using OysterFx.AppCore.AppServices.Commands;
using OysterFx.AppCore.Shared.Commands.Common;
using Suspect.TaskPro.AppCore.Shared.Tasks.LogWorks.Commands.AddWorkLog;
using Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Commands;
using System;
using System.Threading.Tasks;

public class AddWorkLogCommandHandler : CommandHandler<AddWorkLogCommand, Guid>
{
    private readonly IWorkItemCommandRepository _workItemCommandRepository;

    public AddWorkLogCommandHandler(IWorkItemCommandRepository workItemCommandRepository)
    {
        _workItemCommandRepository = workItemCommandRepository;
    }

    public override async Task<CommandResult<Guid>> Handle(AddWorkLogCommand command)
    {
        var workItem = await _workItemCommandRepository.GetGraphAsync(command.WorkItemBusinessKey);
        workItem.AddWorkLog(
            command.HoursSpent,
            command.WorkDate,
            command.LoggedByResourceBusinessKey,
            command.Description);

        await _workItemCommandRepository.CommitAsync();
        return await OkAsync(workItem.BusinessKey.Value);
    }
}