using BSI.Core.Extensions;
using BSI.Core.Objects;
using TaleWorlds.ObjectSystem;

namespace BSI.CivilWar.Goals.WarForIndependence
{
    class WarForIndependenceGoal : Goal
    {
        public WarForIndependenceGoal(MBObjectBase target, Behavior behavior) : base (target, behavior)
        {
            this.Manifesto = $"War of Independence from {this.Plot.Target.ConvertToKingdom()}";
        }
    }
}
