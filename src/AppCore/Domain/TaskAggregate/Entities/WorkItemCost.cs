namespace Suspect.TaskPro.AppCore.Domain.TaskAggregate.Entities;

using OysterFx.AppCore.Domain.Aggregates;
using OysterFx.AppCore.Domain.ValueObjects;
using Suspect.TaskPro.AppCore.Domain.TaskAggregate.Enums;

public class WorkItemCost : Aggregate
{
    public BusinessKey WorkItemBusinessKey { get; private set; }
    public CostType Type { get; private set; }
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = "IRR";
    public string Description { get; private set; } = string.Empty;
    public DateTime CostDate { get; private set; }
    public DateTime CreatedDate { get; private set; } = DateTime.UtcNow;
    public CostStatus Status { get; private set; } = CostStatus.Planned;
    public long? ApprovedByResourceId { get; private set; }
    public DateTime? ApprovedDate { get; private set; }
    public string? ReferenceNumber { get; private set; }
    public string? Notes { get; private set; }

    public WorkItem WorkItem { get; private set; } = null!;
    public ICollection<WorkItemCostAttachment> Attachments { get; private set; } = new List<WorkItemCostAttachment>();

    private WorkItemCost() { }

    protected WorkItemCost(
        BusinessKey workItemBusinessKey,
        CostType type,
        decimal amount,
        string currency,
        string description,
        DateTime costDate,
        string? referenceNumber = null,
        string? notes = null)
    {
        ValidateAmount(amount);
        ValidateCurrency(currency);

        WorkItemBusinessKey = workItemBusinessKey;
        Type = type;
        Amount = amount;
        Currency = currency;
        Description = description;
        CostDate = costDate;
        ReferenceNumber = referenceNumber;
        Notes = notes;
    }

    public static WorkItemCost CreateInstance(
        BusinessKey workItemBusinessKey,
        CostType type,
        decimal amount,
        string currency,
        string description,
        DateTime costDate,
        string? referenceNumber = null,
        string? notes = null) => new(
            workItemBusinessKey,
            type,
            amount,
            currency,
            description,
            costDate,
            referenceNumber,
            notes);

    public void Approve(long approvedByResourceId)
    {
        if (Status != CostStatus.Planned)
            throw new InvalidOperationException("Only planned costs can be approved");

        Status = CostStatus.Approved;
        ApprovedByResourceId = approvedByResourceId;
        ApprovedDate = DateTime.UtcNow;
    }

    public void MarkAsInvoiced(string invoiceNumber)
    {
        if (Status != CostStatus.Approved)
            throw new InvalidOperationException("Only approved costs can be marked as invoiced");

        Status = CostStatus.Invoiced;
        ReferenceNumber = invoiceNumber;
    }

    public void MarkAsPaid()
    {
        if (Status != CostStatus.Invoiced)
            throw new InvalidOperationException("Only invoiced costs can be marked as paid");

        Status = CostStatus.Paid;
    }

    public void Reject(string reason)
    {
        Status = CostStatus.Rejected;
        Notes = $"{Notes}\nRejected: {reason}";
    }

    public void UpdateAmount(decimal newAmount, string reason)
    {
        ValidateAmount(newAmount);
        Amount = newAmount;
        Notes = $"{Notes}\nAmount updated: {reason}";
    }

    private static void ValidateAmount(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Cost amount must be positive");
    }

    private static void ValidateCurrency(string currency)
    {
        if (string.IsNullOrWhiteSpace(currency) || currency.Length != 3)
            throw new ArgumentException("Currency must be 3-letter code");
    }
}