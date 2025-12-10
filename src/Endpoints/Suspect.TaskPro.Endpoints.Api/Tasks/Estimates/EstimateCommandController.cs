namespace Suspect.TaskPro.Endpoints.Api.Tasks.Estimates;

using Microsoft.AspNetCore.Mvc;
using OysterFx.Endpoints.Api.Controllers;
using Suspect.TaskPro.AppCore.Shared.Tasks.Estimates.CreateRevisionEstimate;
using Suspect.TaskPro.AppCore.Shared.Tasks.Estimates.InitialEstimate;

[ApiExplorerSettings(GroupName = "Tasks")]
[Route("api/Tasks/[controller]")]
public class EstimateCommandController : OysterFxController
{
    [HttpPost("[action]")]
    public async Task<IActionResult> InitialEstimate([FromBody] InitialEstimateCommand command)
        => await SendCommand<InitialEstimateCommand, Guid>(command);
    [HttpPost("[action]")]
    public async Task<IActionResult> RevisionEstimate([FromBody] RevisionEstimateCommand command)
        => await SendCommand<RevisionEstimateCommand, Guid>(command);    
}