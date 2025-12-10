namespace Suspect.TaskPro.AppCore.AppServices.Tasks.Estimates.Commands.InitialEstimate;

using OysterFx.AppCore.AppServices.Commands;
using OysterFx.AppCore.Shared.Commands.Common;
using Suspect.TaskPro.AppCore.Shared.Tasks.Estimates.CreateRevisionEstimate;
using Suspect.TaskPro.AppCore.Shared.Tasks.Estimates.InitialEstimate;
using Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Commands;

public class InitialEstimateCommandHandler : CommandHandler<InitialEstimateCommand, Guid>
{
    private readonly IWorkItemCommandRepository _workItemCommandRepository;

    public InitialEstimateCommandHandler(IWorkItemCommandRepository workItemCommandRepository)
    {
        _workItemCommandRepository = workItemCommandRepository;
    }

    public override async Task<CommandResult<Guid>> Handle(InitialEstimateCommand command)
    {
        var workItem = await _workItemCommandRepository.GetGraphAsync(command.WorkItemBusinessKey);

        workItem.SetInitialEstimate(
            command.OptimisticHours,
             command.MostLikelyHours,
             command.PessimisticHours,
             command.ConfidenceLevel,
             command.EstimatedByResourceBusinessKey!,
             command.Assumptions);

        await _workItemCommandRepository.CommitAsync();
        return await OkAsync(workItem.BusinessKey.Value);
    }
}