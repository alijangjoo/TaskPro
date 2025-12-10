namespace Suspect.TaskPro.AppCore.AppServices.Tasks.CheckLists.Commands.AddCheckList;

using OysterFx.AppCore.AppServices.Commands;
using OysterFx.AppCore.Shared.Commands.Common;
using Suspect.TaskPro.AppCore.Shared.Tasks.CheckLists.Commands.AddCheckList;
using Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Commands;
using System;
using System.Threading.Tasks;

public class AddCheckListCommandHandler : CommandHandler<AddCheckListCommand, Guid>
{
    private readonly IWorkItemCommandRepository _workItemCommandRepository;

    public AddCheckListCommandHandler(IWorkItemCommandRepository workItemCommandRepository)
    {
        _workItemCommandRepository = workItemCommandRepository;
    }

    public override async Task<CommandResult<Guid>> Handle(AddCheckListCommand command)
    {
        var workItem = await _workItemCommandRepository.GetGraphAsync(command.WorkItemBusinessKey);
        workItem.AddCheckList(command.Title!);
        await _workItemCommandRepository.CommitAsync();
        return await OkAsync(workItem.BusinessKey.Value);
    }
}