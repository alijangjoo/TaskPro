namespace Suspect.TaskPro.AppCore.AppServices.IntegrationTests.AAA.Commands.LoginByOtp;

using Suspect.TaskPro.AppCore.AppServices.AAA.Commands.LoginByOtp;
using Suspect.TaskPro.AppCore.AppServices.IntegrationTests.AAA.Commands.Common;
using Suspect.TaskPro.AppCore.DomainServices.OtpCodeServices;
using Suspect.TaskPro.AppCore.Shared.AAA.Commands.LoginByOtp;
using Suspect.TaskPro.Infra.Persistence.Postgres.Commands.OtpCodes;
using System.Threading.Tasks;

public class LoginByOtpPostgresTests : IClassFixture<PostgresFixture>
{
    private readonly PostgresFixture _postgresFixture;

    public LoginByOtpPostgresTests(PostgresFixture postgresFixture)
    {
        _postgresFixture = postgresFixture;
    }

    [Fact]
    public async Task LoginByOtp_ShouldPersistOtpInDb()
    {
        // Arrange
        using var context = TaskProDbContextFactory.Create(_postgresFixture.Container.GetConnectionString());
        var otpRepository = new OtpCodeCommandRepository(context);
        var otpService = new OtpGeneratorDomainService(otpRepository);
        var smsSender = new FakePayamakSender();
        var handler = new LoginByOtpCommandHandler(smsSender, otpService);
        var command = new LoginByOtpCommand("09395600650");

        // Act
        var result = await handler.Handle(command);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Data!.OtpSended);

        // Assert - Database persistence
        var otpInDb = context.OtpCodes
            .SingleOrDefault(o => o.MobileNumber == command.MobileNumber);

        Assert.NotNull(otpInDb);
        Assert.False(otpInDb.IsExpired());
        Assert.Equal("09395600650", otpInDb.MobileNumber);
    }
}