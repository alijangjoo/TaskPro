namespace Suspect.TaskPro.AppCore.Shared.Tasks.CheckLists.Commands.RemoveCheckList;

using OysterFx.AppCore.Shared.Commands;
using System;

public class RemoveCheckListCommand : ICommand<bool>
{
    public Guid WorkItemBusinessKey { get; set; }
    public Guid CkeckListBusinessKey { get; set; }
}