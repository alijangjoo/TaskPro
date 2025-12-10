namespace Suspect.TaskPro.AppCore.AppServices.Tasks.LogWorks.Commands.RemoveWorkLog;

using OysterFx.AppCore.AppServices.Commands;
using OysterFx.AppCore.Shared.Commands.Common;
using Suspect.TaskPro.AppCore.Shared.Tasks.LogWorks.Commands.RemoveWorkLog;
using Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Commands;
using System.Threading.Tasks;

public class RemoveWorkLogCommandHandler : CommandHandler<RemoveWorkLogCommand, bool>
{
    private readonly IWorkItemCommandRepository _workItemCommandRepository;

    public RemoveWorkLogCommandHandler(IWorkItemCommandRepository workItemCommandRepository)
    {
        _workItemCommandRepository = workItemCommandRepository;
    }

    public override async Task<CommandResult<bool>> Handle(RemoveWorkLogCommand command)
    {
        var workItem = await _workItemCommandRepository.GetGraphAsync(command.WorkItemBusinessKey);
        workItem.RemoveWorkLog(command.LogWorkBusinessKey);
        await _workItemCommandRepository.CommitAsync();
        return await OkAsync(true);
    }
}