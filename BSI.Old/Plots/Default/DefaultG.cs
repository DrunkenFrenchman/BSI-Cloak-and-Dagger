using BSI.Core.Flags;
using BSI.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace BSI.Plots.Default
{
    class DefaultG : Goal
    {
        public DefaultG(IFaction target, AvailableGoals nextGoal = 0) : base(target, nextGoal)
        {
            this.Behavior = new DefaultB();
        }

        public override Behavior Behavior { get; internal set; }

        public override string Manifesto => "stop plotting";
    }
}
