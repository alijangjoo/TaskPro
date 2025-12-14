namespace Suspect.TaskPro.AppCore.AppServices.Tests.TaskServices.WorkItems.Commands.DoneWorkItem;

using Moq;
using Suspect.TaskPro.AppCore.AppServices.TaskServices.WorkItems.Commands.DoneWorkItem;
using Suspect.TaskPro.AppCore.Domain.TaskAggregate.Entities;
using Suspect.TaskPro.AppCore.Domain.TaskAggregate.Enums;
using Suspect.TaskPro.AppCore.Shared.TaskContracts.WorkItems.Commands.DoneWorkItem;
using Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Commands;
using System.Reflection.Metadata;

public class DoneWorkItemCommandHandlerTests
{
    private readonly Mock<IWorkItemCommandRepository> _workItemsCommandRepositoryMock;
    private readonly DoneWorkItemCommandHandler _doneWorkItemCommandHandler;
    public DoneWorkItemCommandHandlerTests()
    {
        _workItemsCommandRepositoryMock = new Mock<IWorkItemCommandRepository>();
        _doneWorkItemCommandHandler = new DoneWorkItemCommandHandler(_workItemsCommandRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Mark_Schedule_As_Done()
    {
        // Arrange
        var scheduleDate = DateTime.UtcNow.AddDays(1);

        var workItem = WorkItem.CreateInstance(
            title: "Test Task",
            recurrenceType: RecurrenceType.None,
            scheduleDates: new List<DateTime> { scheduleDate }
        );

        var scheduleBusinessKey = workItem.Schedules.First().BusinessKey.Value;

        var command = DoneWorkItemCommand.CreateInstance(
            workItem.BusinessKey.Value,
            scheduleBusinessKey
        );

        _workItemsCommandRepositoryMock
            .Setup(r => r.GetGraphAsync(workItem.BusinessKey.Value))
            .ReturnsAsync(workItem);

        // Act
        var result = await _doneWorkItemCommandHandler.Handle(command);

        // Assert
        Assert.True(result.IsSuccess); 

        var schedule = workItem.Schedules.First();

        _workItemsCommandRepositoryMock.Verify(r => r.CommitAsync(), Times.Once);
    }
}