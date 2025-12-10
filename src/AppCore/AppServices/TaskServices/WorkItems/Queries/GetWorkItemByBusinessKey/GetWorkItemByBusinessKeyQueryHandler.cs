namespace Suspect.TaskPro.AppCore.AppServices.TaskServices.WorkItems.Queries.GetWorkItemByBusinessKey;

using OysterFx.AppCore.AppServices.Queries;
using OysterFx.AppCore.Shared.Queries.Common;
using Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Queries;
using Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Queries.GetWorkItemByBusinessKey;
using System.Threading.Tasks;

public class GetWorkItemByBusinessKeyQueryHandler : QueryHandler<GetWorkItemByBusinessKeyQuery, GetWorkItemByBusinessKeyQueryResult?>
{
    private readonly IWorkItemQueryRepository _workItemQueryRepository;

    public GetWorkItemByBusinessKeyQueryHandler(IWorkItemQueryRepository workItemQueryRepository)
    {
        _workItemQueryRepository = workItemQueryRepository;
    }

    public override async Task<QueryResult<GetWorkItemByBusinessKeyQueryResult?>> ExecuteAsync(GetWorkItemByBusinessKeyQuery request)
    {
        var queryResult = await _workItemQueryRepository.QueryAsync(request);
        return await AsQueryResult(queryResult);
    }
}