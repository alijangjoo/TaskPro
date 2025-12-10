namespace Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Queries.GetWorkItemByBusinessKey;

using OysterFx.AppCore.Shared.Queries;
using System;

public class GetWorkItemByBusinessKeyQuery : IQuery<GetWorkItemByBusinessKeyQueryResult?>
{
    public Guid BusinessKey { get; set; }
    public GetWorkItemByBusinessKeyQuery(Guid businessKey)
    {
        BusinessKey = businessKey;
    }
    public static GetWorkItemByBusinessKeyQuery CreateInstance(Guid businessKey) => new GetWorkItemByBusinessKeyQuery(businessKey);
}