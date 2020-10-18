using BSI.Core.Extensions;
using BSI.Core.Objects;
using System.Collections.Generic;
using TaleWorlds.ObjectSystem;

namespace BSI.CivilWar.Goals.WarForIndependence
{
    class WarForIndependenceGoal : Goal
    {
        public WarForIndependenceGoal(MBObjectBase target, Behavior behavior) : base (target, behavior)
        {

        }

        public WarForIndependenceGoal(MBObjectBase target, Behavior behavior, List<Goal> goals, string manifesto = "Plot against ") : base(target, behavior, goals, manifesto)
        {

        }

        public override bool EndCondition => !this.GetPlot.Leader.ConvertToHero().MapFaction.Equals(GetPlot.Target.ConvertToHero().MapFaction)
            && !this.GetPlot.Leader.ConvertToHero().MapFaction.IsAtWarWith(GetPlot.Target.ConvertToHero().MapFaction);

        public override string Manifesto => "War of Independence from " + GetPlot.Target.ConvertToKingdom();
    }
}
