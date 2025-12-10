namespace Suspect.TaskPro.AppCore.AppServices.TaskServices.WorkItems.Queries.GetAllWorkItems;

using OysterFx.AppCore.AppServices.Queries;
using OysterFx.AppCore.Shared.Queries.Common;
using Suspect.TaskPro.AppCore.Shared.TaskContracts.WorkItems.Queries.GetAllWorkItems;
using Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

public class GetAllWorkItemsQueryHandler : QueryHandler<GetAllWorkItemsQuery, List<GetAllWorkItemsQueryResult?>>
{
    private readonly IWorkItemQueryRepository _workItemQueryRepository;

    public GetAllWorkItemsQueryHandler(IWorkItemQueryRepository workItemQueryRepository)
    {
        _workItemQueryRepository = workItemQueryRepository;
    }

    public override async Task<QueryResult<List<GetAllWorkItemsQueryResult?>>> ExecuteAsync(GetAllWorkItemsQuery request)
    {
        var queryResult = await _workItemQueryRepository.QueryAsync(request);
        return await AsQueryResult(queryResult);
    }
}