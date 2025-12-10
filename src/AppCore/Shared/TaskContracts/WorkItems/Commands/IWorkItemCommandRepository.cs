namespace Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Commands;

using OysterFx.AppCore.Shared.Commands;
using Suspect.TaskPro.AppCore.Domain.TaskAggregate.Entities;

public interface IWorkItemCommandRepository : ICommandRepository<WorkItem, long>
{
}
