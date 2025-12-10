namespace Suspect.TaskPro.AppCore.Shared.Tasks.LogWorks.Commands.AddWorkLog;

using OysterFx.AppCore.Domain.ValueObjects;
using OysterFx.AppCore.Shared.Commands;
using System;

public class AddWorkLogCommand : ICommand<Guid>
{
    public Guid WorkItemBusinessKey { get; set; }
    public double HoursSpent { get; set; }
    public DateTime WorkDate { get; set; }
    public Guid LoggedByResourceBusinessKey { get; set; } = default!;
    public string? Description { get; set; }
}