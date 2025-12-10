namespace Suspect.TaskPro.Infra.Persistence.Postgres.Queries.Tasks.Entities;

public class WorkItemSchedule
{
    public long Id { get; set; }
    public Guid BusinessKey { get; set; }
    public Guid WorkItemBusinessKey { get;  set; }
    public DateTime ScheduledDate { get;  set; }
    public bool IsDone { get; set; }
}