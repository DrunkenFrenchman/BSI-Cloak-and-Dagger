using BSI.Core.Flags;
using BSI.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace BSI.Plots
{
    class WarforIndependenceG : Goal
    {
        public WarforIndependenceG(IFaction target, AvailableGoals nextGoal = AvailableGoals.Default) : base(target, nextGoal)
        {

        }

        public override Behavior Behavior { get => throw new NotImplementedException(); internal set => throw new NotImplementedException(); }

        public override string Name => base.Name;

        public override string Manifesto => throw new NotImplementedException();
    }
}
