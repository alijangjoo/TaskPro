namespace Suspect.TaskPro.Infra.Persistence.Postgres.Commands.Tasks.WorkItems;

using OysterFx.AppCore.Domain.Aggregates;
using OysterFx.AppCore.Domain.ValueObjects;
using OysterFx.Infra.Persistence.RDB.Commands;
using Suspect.TaskPro.AppCore.Domain.TaskAggregate.Entities;
using Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Commands;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;

public class WorkItemCommandRepository : CommandRepository<WorkItem, AppCommandDbContext>, IWorkItemCommandRepository
{
    public WorkItemCommandRepository(AppCommandDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> DeleteGraphByBusinessKeyAsync(BusinessKey businessKey)
    {
        var entity = GetGraph(businessKey);
        if (entity is not null && !entity.Id.Equals(default))
            _dbContext.Set<WorkItem>().Remove(entity);

        return await Task.FromResult(true);
    }
}