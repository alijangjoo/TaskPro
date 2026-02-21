namespace Suspect.TaskPro.AppCore.Shared.TaskContracts.WorkItems.Commands.EditWorkItem;

using OysterFx.AppCore.Shared.Commands;
using Suspect.TaskPro.AppCore.Domain.TaskAggregate.Enums;
using System;
using System.Collections.Generic;

public class EditWorkItemCommand : ICommand<Guid>
{
    public Guid BusinessKey { get; set; }
    public string? Title { get; set; }
    public RecurrenceType RecurrenceType { get; set; }
    public List<DateTime>? ScheduleDates { get; set; }
}