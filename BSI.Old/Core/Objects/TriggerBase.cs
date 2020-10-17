using BSI.Core.Flags;
using BSI.Core.Managers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace BSI.Core.Objects
{
    
    public abstract class TriggerBase
    {
        public TriggerBase()
        {

        }
        //public abstract ReadOnlyCollection<Goal> PotentialEndGoals { get; internal set; }
        public abstract Uniqueto Uniqueto { get; internal set; }                                                                       
        public abstract bool CanPlot(Hero hero);
        public abstract bool WantPlot(Hero hero);
        public abstract bool DoPlot(Hero hero);
    }
}
