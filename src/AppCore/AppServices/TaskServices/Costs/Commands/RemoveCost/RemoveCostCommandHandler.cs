namespace Suspect.TaskPro.AppCore.AppServices.Tasks.Costs.Commands.RemoveCost;

using OysterFx.AppCore.AppServices.Commands;
using OysterFx.AppCore.Shared.Commands.Common;
using Suspect.TaskPro.AppCore.Shared.Tasks.Costs.Commands.RemoveCost;
using Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Commands;
using System.Threading.Tasks;

public class RemoveCostCommandHandler : CommandHandler<RemoveCostCommand, bool>
{
    private readonly IWorkItemCommandRepository _workItemCommandRepository;

    public RemoveCostCommandHandler(IWorkItemCommandRepository workItemCommandRepository)
    {
        _workItemCommandRepository = workItemCommandRepository;
    }

    public override async Task<CommandResult<bool>> Handle(RemoveCostCommand command)
    {
        var workItem = await _workItemCommandRepository.GetGraphAsync(command.WorkItemBusinessGuid);
        workItem.RemoveCost(command.CostBusinessKey);
        await _workItemCommandRepository.CommitAsync();
        return await OkAsync(true);
    }
}