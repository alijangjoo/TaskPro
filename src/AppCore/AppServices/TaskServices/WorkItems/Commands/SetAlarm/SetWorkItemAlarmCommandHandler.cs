using OysterFx.AppCore.AppServices.Commands;
using OysterFx.AppCore.Shared.Commands.Common;
using Suspect.TaskPro.AppCore.Shared.TaskContracts.CheckLists.Commands.SetAlarm;
using Suspect.TaskPro.AppCore.Shared.Tasks.WorkItems.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suspect.TaskPro.AppCore.AppServices.TaskServices.WorkItems.Commands.SetAlarm
{
    public class SetWorkItemAlarmCommandHandler
    : CommandHandler<SetWorkItemAlarmCommand, bool>
    {
        private readonly IWorkItemCommandRepository _repo;

        public SetWorkItemAlarmCommandHandler(IWorkItemCommandRepository repo)
        {
            _repo = repo;
        }

        public override async Task<CommandResult<bool>> Handle(SetWorkItemAlarmCommand command)
        {
            var workItem = await _repo.GetAsync(command.BusinessKey);

            if (workItem == null)
                return await FailAsync("WorkItem not found");

            workItem.SetAlarm(command.AlarmTime);

            await _repo.CommitAsync();

            return await OkAsync(true);
        }
    }
}
