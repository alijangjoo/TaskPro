namespace Suspect.TaskPro.AppCore.AppServices.TaskServices.WorkItems.Commands.DeleteWorkItem;

using OysterFx.AppCore.AppServices.Commands;
using OysterFx.AppCore.Shared.Commands.Common;
using Suspect.TaskPro.AppCore.Shared.TaskContracts.WorkItems.Commands.DeleteWorkItem;
using Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Commands;
using System.Threading.Tasks;

public class DeleteWorkItemCommandHandler : CommandHandler<DeleteWorkItemCommand, bool>
{
    private readonly IWorkItemCommandRepository _workItemCommandRepository;

    public DeleteWorkItemCommandHandler(IWorkItemCommandRepository workItemCommandRepository)
    {
        _workItemCommandRepository = workItemCommandRepository;
    }

    public override async Task<CommandResult<bool>> Handle(DeleteWorkItemCommand command)
    {
        await _workItemCommandRepository.DeleteGraphByBusinessKeyAsync(command.WorkItemBusinessKey);
        await _workItemCommandRepository.CommitAsync();
        return await OkAsync(true);
    }
}
