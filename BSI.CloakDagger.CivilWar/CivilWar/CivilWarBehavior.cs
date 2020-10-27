using System.Linq;
using BSI.CloakDagger.CivilWar.CivilWar.Goals.RecruitForWar;
using BSI.CloakDagger.CivilWar.CivilWar.Goals.WarForIndependence;
using BSI.CloakDagger.Managers;
using TaleWorlds.CampaignSystem;

namespace BSI.CloakDagger.CivilWar.CivilWar
{
    public class CivilWarBehavior : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, OnDailyTick);
        }

        public override void SyncData(IDataStore dataStore)
        {
        }

        private void OnDailyTick()
        {
            foreach (var plot in GameManager.Instance.PlotManager.SelectMany(p => p.Value).Where(p => p.TriggerType.GetType() == typeof(CivilWarTrigger)))
            {
                var behavior = plot.ActiveGoal.Behavior;

                switch (behavior)
                {
                    case RecruitForWarBehavior recruitForWarBehavior:
                        recruitForWarBehavior.DailyTick();
                        break;
                    case WarForIndependenceBehavior warForIndependenceBehavior:
                        warForIndependenceBehavior.DailyTick();
                        break;
                }
            }
        }
    }
}