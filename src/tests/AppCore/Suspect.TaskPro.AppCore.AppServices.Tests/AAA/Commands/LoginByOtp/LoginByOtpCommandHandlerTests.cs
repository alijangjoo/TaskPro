namespace Suspect.TaskPro.AppCore.AppServices.Tests.AAA.Commands.LoginByOtp;

using Moq;
using Supect.TaskPro.Infra.PanelPayamak.Abstractions;
using Supect.TaskPro.Infra.PanelPayamak.Abstractions.Dtos;
using Suspect.TaskPro.AppCore.AppServices.AAA.Commands.LoginByOtp;
using Suspect.TaskPro.AppCore.Shared.AAA.Commands.LoginByOtp;
using Suspect.TaskPro.AppCore.Shared.OtpCodeContracts.OtpCodes.DomainServices;


public class LoginByOtpCommandHandlerTests
{
    private readonly Mock<IPayamakSender> _payamakSenderMock;
    private readonly Mock<IOtpGeneratorDomainService> _otpGeneratorDomainServiceMock;
    private readonly LoginByOtpCommandHandler _loginByOtpCommandHandler;

    public LoginByOtpCommandHandlerTests()
    {
        _payamakSenderMock = new Mock<IPayamakSender>();
        _otpGeneratorDomainServiceMock = new Mock<IOtpGeneratorDomainService>();
        _loginByOtpCommandHandler = new LoginByOtpCommandHandler(_payamakSenderMock.Object, _otpGeneratorDomainServiceMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Send_Otp_And_Return_Success()
    {

        // Arrange
        var loginByOtpCommand = new LoginByOtpCommand("09395600650");

        _otpGeneratorDomainServiceMock
            .Setup(o => o.ExecuteAsync(loginByOtpCommand.MobileNumber!, It.IsAny<int>()))
            .ReturnsAsync("1234");

        _payamakSenderMock.Setup(s => s.ExecuteAsync(It.IsAny<PayamakInputDto>()))
                      .ReturnsAsync(new PayamakResultDto
                      {
                          IsSuccess = true,
                          Message = "Sent"
                      });
        // Act
        var result = await _loginByOtpCommandHandler.Handle(loginByOtpCommand);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Data!.OtpSended);
    }

    [Fact]
    public async Task Handle_WhenEveryThingIsOk_ShouldSendOtpAndReturnSuccess()
    {
        // Arrange
        var command = new LoginByOtpCommand("09395600650");

        _otpGeneratorDomainServiceMock
            .Setup(o => o.ExecuteAsync(command.MobileNumber!, It.IsAny<int>()))
            .ReturnsAsync("12345");

        _payamakSenderMock
            .Setup(p => p.ExecuteAsync(It.IsAny<PayamakInputDto>()))
            .ReturnsAsync(new PayamakResultDto()
            {
                IsSuccess = true,
                Message = "Sent"
            });

        // Act
        var commandResult = await _loginByOtpCommandHandler.Handle(command);

        // Assert
        Assert.True(commandResult.IsSuccess);
        Assert.True(commandResult.Data!.OtpSended);

        _otpGeneratorDomainServiceMock.Verify(o => o.ExecuteAsync(command.MobileNumber!, It.IsAny<int>()), Times.Once);
        _payamakSenderMock.Verify(p => p.ExecuteAsync(It.Is<PayamakInputDto>(dto => dto.Receptor == command.MobileNumber && dto.Message == "12345")), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenSmsSendingFail_ShouldReturnFailure()
    {
        // Arrange
        var command = new LoginByOtpCommand("09395600650");
        _otpGeneratorDomainServiceMock
            .Setup(o => o.ExecuteAsync(command.MobileNumber!, It.IsAny<int>()))
            .ReturnsAsync("12345");

        _payamakSenderMock
            .Setup(p => p.ExecuteAsync(It.IsAny<PayamakInputDto>()))
            .ReturnsAsync(new PayamakResultDto()
            {
                IsSuccess = false,
                Message = "sent failed"
            });

        // Act
        var commandResult = await _loginByOtpCommandHandler.Handle(command);

        // Assert

        Assert.False(commandResult.IsSuccess);
    }
}