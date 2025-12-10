namespace Suspect.TaskPro.AppCore.Domain.TaskAggregate.Enums;

public enum CostStatus
{
    Planned = 1,    // برنامه‌ریزی شده
    Approved = 2,   // تأیید شده
    Invoiced = 3,   // فاکتور شده
    Paid = 4,       // پرداخت شده
    Rejected = 5    // رد شده
}