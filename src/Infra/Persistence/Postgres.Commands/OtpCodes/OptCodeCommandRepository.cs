namespace Suspect.TaskPro.Infra.Persistence.Postgres.Commands.OtpCodes;

using OysterFx.Infra.Persistence.RDB.Commands;
using Suspect.TaskPro.AppCore.Domain.OtpCodesAggregate.Entities;
using Suspect.TaskPro.AppCore.Shared.OtpCodeContracts.OtpCodes.Commands;
using System.Linq;
using System.Threading.Tasks;

public class OtpCodeCommandRepository : CommandRepository<OtpCode, AppCommandDbContext>, IOtpCodeCommandRepository
{
    public OtpCodeCommandRepository(AppCommandDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<OtpCode> GetByMobileNumberAndCode(string mobileNumber, string code)
    {
        var otpCode = _dbContext.OtpCodes.FirstOrDefault(c => c.MobileNumber == mobileNumber && c.Code == code);
        return await Task.FromResult(otpCode!);
    }
}