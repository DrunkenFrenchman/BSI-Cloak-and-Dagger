using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library.EventSystem;

namespace BSI.Core.Events
{
    public sealed class PlotEvent : MBCampaignEvent
    {
        public PlotEvent(string eventName) : base(eventName)
        {
        }

        public PlotEvent(CampaignTime triggerPeriod, CampaignTime initialWait) : base(triggerPeriod, initialWait)
        {
        }
    }
}
