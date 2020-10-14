using BSI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace BSI.Plots.CivilWar
{
    public sealed class RecruitforCivilWarG : Goal
    {
        public RecruitforCivilWarG(IFaction target, Goal nextGoal = null) : base(target, nextGoal)
        {

        }

        public override string Name => base.Name;

        public override string Manifesto => base.Manifesto;

        protected override BehaviorCore Behavior => new BSI.Plots.CivilWar.RecruitforCivilWarB(this);
    }
}
