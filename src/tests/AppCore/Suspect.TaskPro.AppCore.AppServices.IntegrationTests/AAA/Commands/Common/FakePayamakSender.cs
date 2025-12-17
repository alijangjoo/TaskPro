namespace Suspect.TaskPro.AppCore.AppServices.IntegrationTests.AAA.Commands.Common;

using Supect.TaskPro.Infra.PanelPayamak.Abstractions;
using Supect.TaskPro.Infra.PanelPayamak.Abstractions.Dtos;

public class FakePayamakSender : IPayamakSender
{
    public Task<PayamakResultDto> ExecuteAsync(PayamakInputDto input)
    {
        return Task.FromResult(new PayamakResultDto
        {
            IsSuccess = true,
            Message = "FAKE-SENT"
        });
    }
}