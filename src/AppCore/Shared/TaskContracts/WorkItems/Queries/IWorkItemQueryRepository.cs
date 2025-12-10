namespace Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Queries;

using OysterFx.AppCore.Shared.Queries;
using Suspect.TaskPro.AppCore.Shared.TaskContracts.WorkItems.Queries.GetAllWorkItems;
using Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Queries.GetWorkItemByBusinessKey;

public interface IWorkItemQueryRepository : IQueryRepository
{
    Task<GetWorkItemByBusinessKeyQueryResult?> QueryAsync(GetWorkItemByBusinessKeyQuery query);
    Task<List<GetAllWorkItemsQueryResult?>> QueryAsync(GetAllWorkItemsQuery query);
}