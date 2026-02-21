namespace Suspect.TaskPro.AppCore.Shared.TaskContracts.WorkItems.Commands.DeleteWorkItem;

using OysterFx.AppCore.Shared.Commands;

public class DeleteWorkItemCommand : ICommand<bool>
{
    public Guid WorkItemBusinessKey { get; set; }
}