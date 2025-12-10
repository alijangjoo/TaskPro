namespace Suspect.TaskPro.AppCore.Domain.TaskAggregate.Enums;

public enum EstimateType
{
    Initial = 1,        // تخمین اولیه
    Revised = 2,        // تجدید نظر شده
    Final = 3,          // نهایی
    Actual = 4          // بر اساس داده‌های واقعی
}
