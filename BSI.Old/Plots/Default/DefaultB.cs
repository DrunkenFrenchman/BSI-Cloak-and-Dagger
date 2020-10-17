using BSI.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace BSI.Plots.Default
{
    class DefaultB : Behavior
    {
        public override bool CanPlot(Hero hero, Plot plot)
        {
            return false;
        }

        public override bool DoPlot(Hero hero, Plot plot)
        {
            return false;
        }

        public override bool EndCondition(Plot plot)
        {
            return false;
        }

        public override bool EndResult(Plot plot)
        {
            return false;
        }

        public override bool IsNewLeader(Hero hero, Plot plot)
        {
            return false;
        }

        public override bool LeaveCondition(Hero hero, Plot plot)
        {
            return false;
        }

        public override bool OnDailyTick(Plot plot)
        {
            return false;
        }

        public override bool WantPlot(Hero hero, Plot plot)
        {
            return false;
        }
    }
}
