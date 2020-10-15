using BSI.Core;
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
        public RecruitforCivilWarG(IFaction target, Goal nextGoal = null) : base(target, nextGoal)
        {
            this.Behavior = new RecruitforCivilWarB();
        }


        public override string Name => base.Name;

        public override string Manifesto => base.Manifesto;

        public override Behavior Behavior { get; internal set; }

    }
}
