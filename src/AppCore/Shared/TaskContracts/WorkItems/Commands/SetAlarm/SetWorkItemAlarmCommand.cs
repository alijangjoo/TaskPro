using OysterFx.AppCore.Shared.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suspect.TaskPro.AppCore.Shared.TaskContracts.CheckLists.Commands.SetAlarm
{
    public class SetWorkItemAlarmCommand : ICommand<bool>
    {
        public Guid BusinessKey { get; set; }
        public DateTime AlarmTime { get; set; }
    }
}
