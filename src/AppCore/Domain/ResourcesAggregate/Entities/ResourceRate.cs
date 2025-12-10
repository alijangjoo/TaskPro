namespace Suspect.TaskPro.AppCore.Domain.ResourcesAggregate.Entities;

using OysterFx.AppCore.Domain.Aggregates;
using OysterFx.AppCore.Domain.ValueObjects;
using Suspect.TaskPro.AppCore.Domain.ResourcesAggregate.Enums;

public class ResourceRate : Aggregate
{
    public BusinessKey ResourceBusinessKey { get; private set; }
    public decimal HourlyRate { get; private set; }
    public string Currency { get; private set; } = "IRR";
    public DateTime EffectiveDate { get; private set; }
    public DateTime? ExpiryDate { get; private set; }
    public RateType Type { get; private set; }

    private ResourceRate() { }

    private ResourceRate(
        BusinessKey resourceBusinessKey,
        decimal hourlyRate,
        string currency,
        DateTime effectiveDate,
        RateType type,
        DateTime? expiryDate = null)
    {
        ResourceBusinessKey = resourceBusinessKey;
        HourlyRate = hourlyRate;
        Currency = currency;
        EffectiveDate = effectiveDate;
        Type = type;
        ExpiryDate = expiryDate;
    }

    public static ResourceRate CreateInstance(
        BusinessKey resourceBusinessKey,
        decimal hourlyRate,
        string currency,
        DateTime effectiveDate,
        RateType type,
        DateTime? expiryDate = null) => new ResourceRate(resourceBusinessKey, hourlyRate, currency, effectiveDate, type);

    public bool IsActive(DateTime date)
    {
        return date >= EffectiveDate && (ExpiryDate == null || date <= ExpiryDate);
    }
}
