namespace Suspect.TaskPro.AppCore.AppServices.Tasks.CheckLists.Commands.RemoveCheckList;

using OysterFx.AppCore.AppServices.Commands;
using OysterFx.AppCore.Shared.Commands.Common;
using Suspect.TaskPro.AppCore.Shared.Tasks.CheckLists.Commands.RemoveCheckList;
using Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Commands;
using System.Threading.Tasks;

public class RemoveCheckListCommandHandler : CommandHandler<RemoveCheckListCommand, bool>
{
    private readonly IWorkItemCommandRepository _workItemCommandRepository;

    public RemoveCheckListCommandHandler(IWorkItemCommandRepository workItemCommandRepository)
    {
        _workItemCommandRepository = workItemCommandRepository;
    }

    public override async Task<CommandResult<bool>> Handle(RemoveCheckListCommand command)
    {
        var workItem = await _workItemCommandRepository.GetGraphAsync(command.WorkItemBusinessKey);
        workItem.RemoveCheckList(command.CkeckListBusinessKey);
        await _workItemCommandRepository.CommitAsync();
        return await OkAsync(true);
    }
}