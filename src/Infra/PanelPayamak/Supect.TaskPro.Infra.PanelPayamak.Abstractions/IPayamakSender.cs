namespace Supect.TaskPro.Infra.PanelPayamak.Abstractions;

using Supect.TaskPro.Infra.PanelPayamak.Abstractions.Dtos;

public interface IPayamakSender
{
    Task<PayamakResultDto> ExecuteAsync(PayamakInputDto input);
}