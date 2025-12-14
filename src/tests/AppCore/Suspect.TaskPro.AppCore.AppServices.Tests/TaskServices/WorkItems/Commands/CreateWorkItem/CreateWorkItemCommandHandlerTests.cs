namespace Suspect.TaskPro.AppCore.AppServices.Tests.TaskServices.WorkItems.Commands.CreateWorkItem;

using Moq;
using Suspect.TaskPro.AppCore.AppServices.Tasks.WorkItems.Commands.CreateWorkItem;
using Suspect.TaskPro.AppCore.Domain.TaskAggregate.Entities;
using Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Commands;
using Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Commands.CreateWorkItem;

public class CreateWorkItemCommandHandlerTests
{
    private readonly Mock<IWorkItemCommandRepository> _workItemCommandRepositoryMock;
    private readonly CreateWorkItemCommandHandler _createWorkItemCommandHandler;
    public CreateWorkItemCommandHandlerTests()
    {
        _workItemCommandRepositoryMock = new Mock<IWorkItemCommandRepository>();
        _createWorkItemCommandHandler = new CreateWorkItemCommandHandler(_workItemCommandRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Create_WorkItem_And_Return_GUID()
    {
        // Arrange
        CreateWorkItemCommand createWorkItemCommand = new CreateWorkItemCommand()
        {
            Title = "Test Title",
            RecurrenceType = Domain.TaskAggregate.Enums.RecurrenceType.Daily,
            ScheduleDates = new List<DateTime>()
            {
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(2)
            }
        };
        WorkItem createWorkItem = default!;

        _workItemCommandRepositoryMock
            .Setup(c => c.InsertAsync(It.IsAny<WorkItem>()))
            .Callback<WorkItem>(w => createWorkItem = w)
            .Returns(Task.CompletedTask);

        // Act
        var commandResult = await _createWorkItemCommandHandler.Handle(createWorkItemCommand);

        // Assert
        Assert.True(commandResult.IsSuccess);
        Assert.NotEqual(Guid.Empty, commandResult.Data);

        _workItemCommandRepositoryMock.Verify(r => r.InsertAsync(It.IsAny<WorkItem>()), Times.Once);
        _workItemCommandRepositoryMock.Verify(r => r.CommitAsync(), Times.Once);

        Assert.NotNull(createWorkItem);
        Assert.Equal("Test Title", createWorkItem.Title);
    }
}