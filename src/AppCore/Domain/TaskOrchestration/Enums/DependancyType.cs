namespace Suspect.TaskPro.AppCore.Domain.TaskOrchestration.Enums;

public enum DependancyType : byte
{
    Unknown = 0,
    StartToStart = 1,
    FinishToStart = 2,
    Etc = 3
}