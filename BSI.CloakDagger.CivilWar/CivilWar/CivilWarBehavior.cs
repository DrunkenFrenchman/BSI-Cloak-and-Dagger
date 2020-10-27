using BSI.CloakDagger.CivilWar.CivilWar;
using BSI.CloakDagger.CivilWar.CivilWar.Goals.RecruitForWar;
using BSI.CloakDagger.CivilWar.CivilWar.Goals.WarForIndependence;
using BSI.CloakDagger.Managers;
using System;
using System.Linq;
using TaleWorlds.CampaignSystem;

namespace BSI.CloakDagger.CivilWar
{
    public class CivilWarBehavior : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(OnDailyTick));
        }

        public override void SyncData(IDataStore dataStore)
        {

        }

        private void OnDailyTick()
        {
            foreach (var plot in GameManager.Instance.PlotManager.SelectMany(p => p.Value).Where(p => p.TriggerTypeName.GetType() == typeof(CivilWarTrigger)))
            {
                var behavior = plot.ActiveGoal.Behavior;

                if(behavior is RecruitForWarBehavior recruitForWarBehavior)
                {
                    recruitForWarBehavior.DailyTick();
                }

                if (behavior is WarForIndependenceBehavior warForIndependenceBehavior)
                {
                    warForIndependenceBehavior.DailyTick();
                }
            }
        }
    }
}