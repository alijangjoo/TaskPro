namespace Suspect.TaskPro.Infra.Persistence.Postgres.Commands.Wrappers;

using OysterFx.Infra.Persistence.RDB.Commands;
using Suspect.TaskPro.AppCore.Domain.WrapperAggregate.Entities;
using Suspect.TaskPro.AppCore.Shared.WrapperAgg.Wrappers.Commands;

public class WrapperCommandRepository : CommandRepository<Wrapper, AppCommandDbContext>, IWrapperCommandRepository
{
    public WrapperCommandRepository(AppCommandDbContext dbContext) : base(dbContext)
    {
    }
}