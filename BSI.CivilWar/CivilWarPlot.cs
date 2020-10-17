using BSI.Core.Enumerations;
using BSI.Core.Objects;
using TaleWorlds.CampaignSystem;

namespace BSI.CivilWar
{
    public class CivilWarPlot : Plot
    {
        public CivilWarPlot(Kingdom target, Hero leader, Goal currentGoal, Goal endGoal, UniqueTo uniqueTo) : base(target, leader, currentGoal, endGoal, uniqueTo)
        {
            base.TriggerType = typeof(CivilWarTrigger);
        }
    }
}