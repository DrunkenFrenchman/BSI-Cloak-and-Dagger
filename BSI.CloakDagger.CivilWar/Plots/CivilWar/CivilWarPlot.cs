using BSI.CloakDagger.Enumerations;
using BSI.CloakDagger.Objects;
using TaleWorlds.CampaignSystem;

namespace BSI.CloakDagger.CivilWar.Plots.CivilWar
{
    public class CivilWarPlot : Plot
    {
        public CivilWarPlot(Kingdom target, Hero leader, Goal currentGoal, Goal endGoal) : base(target, leader, currentGoal, endGoal)
        {
            UniqueTo = UniqueTo.Kingdom;
            TriggerType = typeof(CivilWarTrigger);
        }
    }
}