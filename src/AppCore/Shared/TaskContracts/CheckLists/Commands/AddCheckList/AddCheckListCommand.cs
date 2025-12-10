namespace Suspect.TaskPro.AppCore.Shared.Tasks.CheckLists.Commands.AddCheckList;

using OysterFx.AppCore.Shared.Commands;

public class AddCheckListCommand : ICommand<Guid>
{
    public Guid WorkItemBusinessKey { get; set; }
    public string? Title { get; set; }
}