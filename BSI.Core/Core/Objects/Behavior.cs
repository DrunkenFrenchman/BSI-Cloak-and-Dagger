using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace BSI.Core.Objects
{
    public abstract class Behavior
    {
        
        public Behavior()
        {

        }

        public abstract bool EndCondition(Plot plot);

        public abstract bool EndResult(Plot plot);

        public abstract bool OnDailyTick(Plot plot);

        public abstract bool CanPlot(Hero hero, Plot plot);

        public abstract bool WantPlot(Hero hero, Plot plot);

        public abstract bool DoPlot(Hero hero, Plot plot);

        public abstract bool IsNewLeader(Hero hero, Plot plot);

        public abstract bool LeaveCondition(Hero hero, Plot plot);
    }
}
