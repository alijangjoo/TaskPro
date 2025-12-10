namespace Suspect.TaskPro.Endpoints.Api.Tasks.LogWorks;

using Microsoft.AspNetCore.Mvc;
using OysterFx.Endpoints.Api.Controllers;
using Suspect.TaskPro.AppCore.Shared.Tasks.LogWorks.Commands.AddWorkLog;
using Suspect.TaskPro.AppCore.Shared.Tasks.LogWorks.Commands.RemoveWorkLog;

[ApiExplorerSettings(GroupName = "Tasks")]
[Route("api/Tasks/[controller]")]
public class LogWorkCommandController : OysterFxController
{
    [HttpPost("[action]")]
    public async Task<IActionResult> AddWorkLog([FromBody] AddWorkLogCommand command)
        => await SendCommand<AddWorkLogCommand, Guid>(command);

    [HttpPost("[action]")]
    public async Task<IActionResult> RemoveWorkLog([FromBody] RemoveWorkLogCommand command)
        => await SendCommand<RemoveWorkLogCommand, bool>(command);
}