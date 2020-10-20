using BSI.CloakDagger.Objects;
using TaleWorlds.CampaignSystem;

namespace BSI.CloakDagger.CivilWar.Plots.CivilWar.Goals.RecruitForWar
{
    internal class RecruitForWarGoal : Goal
    {
        internal RecruitForWarGoal(CivilWarPlot plot, Kingdom target, Behavior behavior) : base(plot, target, behavior)
        {
            Behavior.Goal = this;
        }
    }
}
