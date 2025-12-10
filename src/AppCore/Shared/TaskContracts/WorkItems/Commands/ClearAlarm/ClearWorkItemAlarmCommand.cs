using OysterFx.AppCore.Shared.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suspect.TaskPro.AppCore.Shared.TaskContracts.WorkItems.Commands.ClearAlarm
{
    public class ClearWorkItemAlarmCommand : ICommand<bool>
    {
        public Guid BusinessKey { get; set; }
    }
}
