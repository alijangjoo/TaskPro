namespace Suspect.TaskPro.AppCore.Shared.OtpCodeContracts.OtpCodes.Commands;

using OysterFx.AppCore.Shared.Commands;
using Suspect.TaskPro.AppCore.Domain.OtpCodesAggregate.Entities;
using System.Threading.Tasks;

public interface IOtpCodeCommandRepository : ICommandRepository<OtpCode, long>
{
    Task<OtpCode> GetByMobileNumberAndCode(string mobileNumber, string code);
}