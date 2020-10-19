using BSI.CloakDagger.Objects;
using TaleWorlds.CampaignSystem;

namespace BSI.CloakDagger.CivilWar.Plots.CivilWar.Goals.RecruitForWar
{
    public class RecruitForWarGoal : Goal
    {
        public RecruitForWarGoal(Kingdom target, Behavior behavior) : base(target, behavior)
        {
            Behavior.Goal = this;
        }
    }
}
