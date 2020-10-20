﻿using TaleWorlds.CampaignSystem;

namespace BSI.CloakDagger.Events
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