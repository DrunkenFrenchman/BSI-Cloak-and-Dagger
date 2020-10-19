using BSI.CloakDagger.Extensions;
using BSI.CloakDagger.Objects;
using TaleWorlds.ObjectSystem;

namespace BSI.CloakDagger.CivilWar.Plots.CivilWar.Goals.WarForIndependence
{
    class WarForIndependenceGoal : Goal
    {
        public WarForIndependenceGoal(MBObjectBase target, Behavior behavior) : base(target, behavior)
        {
            Manifesto = $"War of Independence from {Plot.Target.ConvertToKingdom()}";
            Behavior.Goal = this;
        }
    }
}
