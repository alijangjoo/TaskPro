namespace Suspect.TaskPro.AppCore.Shared.WrapperAgg.Wrappers.Commands;

using OysterFx.AppCore.Shared.Commands;
using Suspect.TaskPro.AppCore.Domain.WrapperAggregate.Entities;

public interface IWrapperCommandRepository : ICommandRepository<Wrapper, long>
{
}