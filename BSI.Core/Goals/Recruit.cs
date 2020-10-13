using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace BSI.Core.Goals
{
    public sealed class Recruit : Goal
    {
        public Recruit(IFaction target, Plot plot, Goal nextGoal = null) : base(target, plot, nextGoal)
        {

        }

        protected override BehaviorCore Behavior => new BSI.Core.Behaviors.Recruit(this.GetPlot);
    }
}
