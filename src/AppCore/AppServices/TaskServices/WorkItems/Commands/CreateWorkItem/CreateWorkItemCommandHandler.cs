namespace Suspect.TaskPro.AppCore.AppServices.Tasks.WorkItems.Commands.CreateWorkItem;

using OysterFx.AppCore.AppServices.Commands;
using OysterFx.AppCore.Shared.Commands.Common;
using Suspect.TaskPro.AppCore.Domain.TaskAggregate.Entities;
using Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Commands;
using Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Commands.CreateWorkItem;
using System;
using System.Threading.Tasks;

public class CreateWorkItemCommandHandler : CommandHandler<CreateWorkItemCommand, Guid>
{
    private readonly IWorkItemCommandRepository _workItemCommandRepository;

    public CreateWorkItemCommandHandler(IWorkItemCommandRepository workItemCommandRepository)
    {
        _workItemCommandRepository = workItemCommandRepository;
    }

    public override async Task<CommandResult<Guid>> Handle(CreateWorkItemCommand command)
    {
        var workItem = WorkItem.CreateInstance(
            command.Title,
            command.RecurrenceType,
            command.ScheduleDates);

        await _workItemCommandRepository.InsertAsync(workItem);
        await _workItemCommandRepository.CommitAsync();
        return await OkAsync(workItem.BusinessKey.Value);
    }
}