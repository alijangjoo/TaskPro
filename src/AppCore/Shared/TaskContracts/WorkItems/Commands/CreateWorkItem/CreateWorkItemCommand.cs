namespace Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Commands.CreateWorkItem;

using OysterFx.AppCore.Shared.Commands;
using Suspect.TaskPro.AppCore.Domain.TaskAggregate.Enums;

public class CreateWorkItemCommand : ICommand<Guid>
{
    public string? Title { get; set; }
    public RecurrenceType RecurrenceType { get; set; }
    public List<DateTime>? ScheduleDates { get; set; }
}