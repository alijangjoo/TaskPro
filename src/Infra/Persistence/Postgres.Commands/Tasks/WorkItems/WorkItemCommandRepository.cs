namespace Suspect.TaskPro.Infra.Persistence.Postgres.Commands.Tasks.WorkItems;

using OysterFx.Infra.Persistence.RDB.Commands;
using Suspect.TaskPro.AppCore.Domain.TaskAggregate.Entities;
using Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Commands;

public class WorkItemCommandRepository : CommandRepository<WorkItem, AppCommandDbContext>, IWorkItemCommandRepository
{
    public WorkItemCommandRepository(AppCommandDbContext dbContext) : base(dbContext)
    {
    }
}