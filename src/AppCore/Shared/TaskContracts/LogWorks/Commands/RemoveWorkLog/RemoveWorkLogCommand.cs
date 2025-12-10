namespace Suspect.TaskPro.AppCore.Shared.Tasks.LogWorks.Commands.RemoveWorkLog;

using OysterFx.AppCore.Shared.Commands;
using System;

public class RemoveWorkLogCommand : ICommand<bool>
{
    public Guid WorkItemBusinessKey { get; set; }
    public Guid LogWorkBusinessKey { get; set; }
}