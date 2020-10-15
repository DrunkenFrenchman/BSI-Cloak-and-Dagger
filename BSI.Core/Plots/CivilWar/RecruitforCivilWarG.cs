using BSI.Core;
using BSI.Core.Flags;
using BSI.Core.Objects;
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
        public RecruitforCivilWarG(IFaction target, AvailableGoals nextGoal = 0) : base(target, nextGoal)
        {
            this.Behavior = new RecruitforCivilWarB();
        }


        public override string Name => base.Name;

        public override string Manifesto => "Overthrow " + this.Target.Leader.Name.ToString() ;

        public override Behavior Behavior { get; internal set; }

    }
}
