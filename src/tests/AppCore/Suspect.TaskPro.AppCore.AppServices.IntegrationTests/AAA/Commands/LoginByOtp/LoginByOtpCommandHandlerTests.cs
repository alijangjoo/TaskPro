namespace Suspect.TaskPro.AppCore.AppServices.IntegrationTests.AAA.Commands.LoginByOtp;

using Suspect.TaskPro.AppCore.AppServices.AAA.Commands.LoginByOtp;
using Suspect.TaskPro.AppCore.AppServices.IntegrationTests.AAA.Commands.Common;
using Suspect.TaskPro.AppCore.DomainServices.OtpCodeServices;
using Suspect.TaskPro.AppCore.Shared.AAA.Commands.LoginByOtp;
using Suspect.TaskPro.Infra.Persistence.Postgres.Commands.OtpCodes;
using System.Threading.Tasks;

public class LoginByOtpCommandHandlerTests
{
    [Fact]
    public async Task LoginByOtp_ShouldGenerateOtp_SaveItAndReturnSuccess()
    {
        // Arrange
        using var appDbContext = TestDbContextFactory.Create();
        var otpCommandRepository = new OtpCodeCommandRepository(appDbContext);
        var otpGeneratorService = new OtpGeneratorDomainService(otpCommandRepository);
        var payamakSenderService = new FakePayamakSender();

        var commandHandler = new LoginByOtpCommandHandler(payamakSenderService, otpGeneratorService);
        var command = new LoginByOtpCommand("09395600650");

        // Act
        var commandResult = await commandHandler.Handle(command);

        // Assert
        Assert.True(commandResult.IsSuccess);
        Assert.True(commandResult.Data!.OtpSended);

        var otpInDb = appDbContext.OtpCodes.FirstOrDefault(c => c.MobileNumber == command.MobileNumber);
        Assert.NotNull(otpInDb);
        Assert.False(otpInDb.IsExpired());
    }
}