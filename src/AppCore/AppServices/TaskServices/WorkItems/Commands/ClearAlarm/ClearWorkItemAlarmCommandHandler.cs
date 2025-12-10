using OysterFx.AppCore.AppServices.Commands;
using OysterFx.AppCore.Shared.Commands.Common;
using Suspect.TaskPro.AppCore.Shared.TaskContracts.WorkItems.Commands.ClearAlarm;
using Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suspect.TaskPro.AppCore.AppServices.TaskServices.WorkItems.Commands.ClearAlarm
{
    public class ClearWorkItemAlarmCommandHandler
    : CommandHandler<ClearWorkItemAlarmCommand, bool>
    {
        private readonly IWorkItemCommandRepository _repo;

        public ClearWorkItemAlarmCommandHandler(IWorkItemCommandRepository repo)
        {
            _repo = repo;
        }

        public override async Task<CommandResult<bool>> Handle(ClearWorkItemAlarmCommand command)
        {
            var workItem = await _repo.GetGraphAsync(command.BusinessKey);

            if (workItem == null)
                return await FailAsync("WorkItem not found");

            workItem.ClearAlarm();

            await _repo.CommitAsync();

            return await OkAsync(true);
        }
    }
}
