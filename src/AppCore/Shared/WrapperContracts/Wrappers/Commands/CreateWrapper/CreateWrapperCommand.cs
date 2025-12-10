namespace Suspect.TaskPro.AppCore.Shared.WrapperAgg.Wrappers.Commands.CreateWrapper;

using OysterFx.AppCore.Shared.Commands;
using Suspect.TaskPro.AppCore.Domain.WrapperAggregate.Enums;

public class CreateWrapperCommand : ICommand<Guid>
{
    public string? Title { get; set; }
    public WrapperType Type { get; set; }
    public long? ParentId { get; set; }
    public long ProgressPercent { get; set; }
    public int Weight { get; set; }
    public string? Description { get; set; }
}