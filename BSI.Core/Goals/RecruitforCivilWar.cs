using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace BSI.Core.Goals
{
    public sealed class RecruitforCivilWar : Goal
    {
        public RecruitforCivilWar(IFaction target, Plot plot, Goal nextGoal = null) : base(target, plot, nextGoal)
        {

        }

        protected override BehaviorCore Behavior => new BSI.Core.Behaviors.RecruitforCivilWar(this);
    }
}
