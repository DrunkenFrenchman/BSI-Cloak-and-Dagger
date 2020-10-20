using BSI.CloakDagger.Objects;
using TaleWorlds.CampaignSystem;

namespace BSI.CloakDagger.CivilWar.Plots.CivilWar.Goals.RecruitForWar
{
    internal class RecruitForWarGoal : Goal
    {
        internal RecruitForWarGoal(Kingdom target, Behavior behavior) : base(target, behavior)
        {
            Behavior.Goal = this;
        }
    }
}
