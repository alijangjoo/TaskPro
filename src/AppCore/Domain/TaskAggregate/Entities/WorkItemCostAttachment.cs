namespace Suspect.TaskPro.AppCore.Domain.TaskAggregate.Entities;

using OysterFx.AppCore.Domain.Aggregates;
using OysterFx.AppCore.Domain.ValueObjects;

public class WorkItemCostAttachment : Aggregate
{
    public BusinessKey CostBusinessKey { get; private set; }
    public string FileName { get; private set; } = string.Empty;
    public string FilePath { get; private set; } = string.Empty;
    public string ContentType { get; private set; } = string.Empty;
    public long FileSize { get; private set; }
    public DateTime UploadDate { get; private set; } = DateTime.UtcNow;
    public long UploadedByResourceId { get; private set; }
    public string? Description { get; private set; }

    private WorkItemCostAttachment() { }

    public WorkItemCostAttachment(
        BusinessKey costBusinessKey,
        string fileName,
        string filePath,
        string contentType,
        long fileSize,
        long uploadedByResourceId,
        string? description = null)
    {
        CostBusinessKey = costBusinessKey;
        FileName = fileName;
        FilePath = filePath;
        ContentType = contentType;
        FileSize = fileSize;
        UploadedByResourceId = uploadedByResourceId;
        Description = description;
    }
}