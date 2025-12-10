namespace Suspect.TaskPro.AppCore.Domain.OtpCodesAggregate.Enums;

public enum OtpStatus : byte
{
    None = 0,
    Active = 1,
    Verified = 2,
    Expired = 3,
    Blocked = 4
}