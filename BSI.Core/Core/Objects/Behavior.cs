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

        public abstract bool EndCondition();

        public abstract bool EndResult();

        public abstract bool OnDailyTick();

        internal abstract bool CanPlot(Hero hero);

        public abstract bool WantPlot(Hero hero);

        public abstract bool DoPlot(Hero hero);

        public abstract bool IsNewLeader(Hero hero);

        public abstract bool LeaveCondition(Hero hero);
    }
}
