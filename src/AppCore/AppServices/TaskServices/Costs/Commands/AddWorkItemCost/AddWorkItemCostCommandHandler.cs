namespace Suspect.TaskPro.AppCore.AppServices.Tasks.Costs.Commands.AddWorkItemCost;

using OysterFx.AppCore.AppServices.Commands;
using OysterFx.AppCore.Shared.Commands.Common;
using Suspect.TaskPro.AppCore.Shared.Tasks.Costs.Commands.AddWorkItemCost;
using Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Commands;
using System;
using System.Threading.Tasks;

public class AddWorkItemCostCommandHandler : CommandHandler<AddWorkItemCostCommand, Guid>
{
    private readonly IWorkItemCommandRepository _workItemCommandRepository;

    public AddWorkItemCostCommandHandler(IWorkItemCommandRepository workItemCommandRepository)
    {
        _workItemCommandRepository = workItemCommandRepository;
    }

    public override async Task<CommandResult<Guid>> Handle(AddWorkItemCostCommand command)
    {
        var workItem = await _workItemCommandRepository.GetGraphAsync(command.WorkItemBusinessKey);
        workItem.AddCost(
            command.Type,
            command.Amount,
            command.Currency!,
            command.Description!,
            command.CostDate,
            command.ReferenceNumber,
            command.Notes);
        await _workItemCommandRepository.CommitAsync();
        return await OkAsync(workItem.BusinessKey.Value);
    }
}