namespace Suspect.TaskPro.Endpoints.Api.Tasks.CheckLists;

using Microsoft.AspNetCore.Mvc;
using OysterFx.Endpoints.Api.Controllers;
using Suspect.TaskPro.AppCore.Shared.Tasks.CheckLists.Commands.AddCheckList;
using Suspect.TaskPro.AppCore.Shared.Tasks.CheckLists.Commands.RemoveCheckList;

[ApiExplorerSettings(GroupName = "Tasks")]
[Route("api/Tasks/[controller]")]
public class CheckListCommandController : OysterFxController
{
    [HttpPost("[action]")]
    public async Task<IActionResult> AddCheckList([FromBody] AddCheckListCommand command)
        => await SendCommand<AddCheckListCommand, Guid>(command);

    [HttpPost("[action]")]
    public async Task<IActionResult> RemoveCheckList([FromBody] RemoveCheckListCommand command)
        => await SendCommand<RemoveCheckListCommand, bool>(command);
}