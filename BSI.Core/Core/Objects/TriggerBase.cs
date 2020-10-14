using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace BSI.Core.Objects
{
    public abstract class TriggerBase
    {
        public abstract bool CanPlot(Hero hero);
        public abstract bool WantPlot(Hero hero);
        public abstract bool DoPlot(Hero hero);
    }
}
