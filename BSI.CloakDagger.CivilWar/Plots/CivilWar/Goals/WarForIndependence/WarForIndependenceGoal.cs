using BSI.CloakDagger.Extensions;
using BSI.CloakDagger.Objects;
using TaleWorlds.ObjectSystem;

namespace BSI.CloakDagger.CivilWar.Plots.CivilWar.Goals.WarForIndependence
{
    internal class WarForIndependenceGoal : Goal
    {
        internal WarForIndependenceGoal(CivilWarPlot plot, MBObjectBase target, Behavior behavior) : base(plot, target, behavior)
        {
            Manifesto = $"War of Independence from {Plot.Target.ConvertToKingdom()}";
            Behavior.Goal = this;
        }
    }
}
