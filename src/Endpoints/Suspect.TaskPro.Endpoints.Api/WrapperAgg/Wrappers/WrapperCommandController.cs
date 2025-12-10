namespace Suspect.TaskPro.Endpoints.Api.WrapperAgg.Wrappers;

using Microsoft.AspNetCore.Mvc;
using OysterFx.Endpoints.Api.Controllers;
using Suspect.TaskPro.AppCore.Shared.WrapperAgg.Wrappers.Commands.CreateWrapper;

[ApiExplorerSettings(GroupName = "Wrappers")]
[Route("api/Wrappers/[controller]")]
public class WrapperCommandController : OysterFxController
{
    [HttpPost("[action]")]
    public async Task<IActionResult> CreateWrapper([FromBody] CreateWrapperCommand command)
        => await SendCommand<CreateWrapperCommand, Guid>(command);
}