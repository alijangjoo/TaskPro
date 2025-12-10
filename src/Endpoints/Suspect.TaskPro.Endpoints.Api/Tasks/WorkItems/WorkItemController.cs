namespace Suspect.TaskPro.Endpoints.Api.Tasks.WorkItems;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OysterFx.Endpoints.Api.Controllers;
using Suspect.TaskPro.AppCore.Shared.TaskContracts.CheckLists.Commands.SetAlarm;
using Suspect.TaskPro.AppCore.Shared.TaskContracts.WorkItems.Commands.ClearAlarm;
using Suspect.TaskPro.AppCore.Shared.TaskContracts.WorkItems.Commands.DoneWorkItem;
using Suspect.TaskPro.AppCore.Shared.TaskContracts.WorkItems.Queries.GetAllWorkItems;
using Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Commands.CreateWorkItem;
using Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Queries.GetWorkItemByBusinessKey;

[Authorize]
//[ApiExplorerSettings(GroupName = "Tasks")]
[Route("api/tasks/work-items")]
public class WorkItemController : OysterFxController
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWorkItemCommand command)
        => await SendCommand<CreateWorkItemCommand, Guid>(command);

    [HttpPost("{businessKey}/done")]
    public async Task<IActionResult> Done([FromRoute] Guid businessKey, [FromBody] Guid scheduleBusinessKey)
        => await SendCommand<DoneWorkItemCommand, bool>(DoneWorkItemCommand.CreateInstance(businessKey, scheduleBusinessKey));

    [HttpGet("{businessKey}")]
    public async Task<IActionResult> Get(Guid businessKey)
        => await ExecuteQueryAsync<GetWorkItemByBusinessKeyQuery, GetWorkItemByBusinessKeyQueryResult?>(GetWorkItemByBusinessKeyQuery.CreateInstance(businessKey));

    [HttpGet("[action]")]
    public async Task<IActionResult> GetAll([FromQuery] GetAllWorkItemsQuery query)
        => await ExecuteQueryAsync<GetAllWorkItemsQuery, List<GetAllWorkItemsQueryResult?>>(query);

    [HttpPost("{businessKey}/alarm/set")]
    public async Task<IActionResult> SetAlarm(Guid businessKey, [FromBody] DateTime alarmTime)
    {
        var cmd = new SetWorkItemAlarmCommand
        {
            BusinessKey = businessKey,
            AlarmTime = alarmTime
        };

        return await SendCommand<SetWorkItemAlarmCommand, bool>(cmd);
    }
    [HttpPost("{businessKey}/alarm/clear")]
    public async Task<IActionResult> ClearAlarm(Guid businessKey)
    {
        var cmd = new ClearWorkItemAlarmCommand
        {
            BusinessKey = businessKey
        };

        return await SendCommand<ClearWorkItemAlarmCommand, bool>(cmd);
    }
}