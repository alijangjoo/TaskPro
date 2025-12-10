namespace Suspect.TaskPro.AppCore.AppServices.TaskServices.WorkItems.Commands.DoneWorkItem;

using OysterFx.AppCore.AppServices.Commands;
using OysterFx.AppCore.Shared.Commands.Common;
using Suspect.TaskPro.AppCore.Shared.TaskContracts.WorkItems.Commands.DoneWorkItem;
using Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Commands;
using System.Threading.Tasks;

public class DoneWorkItemCommandHandler : CommandHandler<DoneWorkItemCommand, bool>
{
    private readonly IWorkItemCommandRepository _workItemCommandRepository;

    public DoneWorkItemCommandHandler(IWorkItemCommandRepository workItemCommandRepository)
    {
        _workItemCommandRepository = workItemCommandRepository;
    }
    public override async Task<CommandResult<bool>> Handle(DoneWorkItemCommand command)
    {
        var workItem = await _workItemCommandRepository.GetGraphAsync(command.WorkItemBusinessKey);
        workItem.MarkScheduleAsDone(command.ScheduleBusinessKey);
        await _workItemCommandRepository.CommitAsync();
        return await OkAsync(true);
    }
}