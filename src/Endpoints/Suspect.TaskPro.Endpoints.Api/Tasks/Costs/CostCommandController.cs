namespace Suspect.TaskPro.Endpoints.Api.Tasks.Costs;

using Microsoft.AspNetCore.Mvc;
using OysterFx.Endpoints.Api.Controllers;
using Suspect.TaskPro.AppCore.Shared.Tasks.Costs.Commands.AddWorkItemCost;
using Suspect.TaskPro.AppCore.Shared.Tasks.Costs.Commands.RemoveCost;

[ApiExplorerSettings(GroupName = "Tasks")]
[Route("api/Tasks/[controller]")]
public class CostCommandController : OysterFxController
{
    [HttpPost("[action]")]
    public async Task<IActionResult> AddWorkItemCost([FromBody] AddWorkItemCostCommand command)
        => await SendCommand<AddWorkItemCostCommand, Guid>(command);

    [HttpPost("[action]")]
    public async Task<IActionResult> RemoveCost([FromBody] RemoveCostCommand command)
        => await SendCommand<RemoveCostCommand, bool>(command);
}