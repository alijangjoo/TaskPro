namespace Suspect.TaskPro.AppCore.AppServices.Tasks.Estimates.Commands.RevisionEstimate;

using OysterFx.AppCore.AppServices.Commands;
using OysterFx.AppCore.Shared.Commands.Common;
using Suspect.TaskPro.AppCore.Shared.Tasks.Estimates.CreateRevisionEstimate;
using Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Commands;
using System;
using System.Threading.Tasks;

public class RevisionEstimateCommandHandler : CommandHandler<RevisionEstimateCommand, Guid>
{
    private readonly IWorkItemCommandRepository _workItemCommandRepository;

    public RevisionEstimateCommandHandler(IWorkItemCommandRepository workItemCommandRepository)
    {
        _workItemCommandRepository = workItemCommandRepository;
    }

    public override async Task<CommandResult<Guid>> Handle(RevisionEstimateCommand command)
    {
        var workItem = await _workItemCommandRepository.GetGraphAsync(command.WorkItemBusinessKey);

        workItem.ReviseEstimate(
            command.OptimisticHours,
             command.MostLikelyHours,
             command.PessimisticHours,
             command.ConfidenceLevel,
             command.Assumptions,
             command.RevisedByResourceBusinessKey!);

        await _workItemCommandRepository.CommitAsync();
        return await OkAsync(workItem.BusinessKey.Value);
    }
}