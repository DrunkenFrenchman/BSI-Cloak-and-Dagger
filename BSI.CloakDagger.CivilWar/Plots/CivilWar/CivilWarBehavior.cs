using BSI.CloakDagger.Managers;
using TaleWorlds.CampaignSystem;

namespace BSI.CloakDagger.CivilWar.Plots.CivilWar
{
    internal class CivilWarBehavior : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, SessionLaunched);
        }

        public override void SyncData(IDataStore dataStore)
        {

        }

        private void SessionLaunched(CampaignGameStarter campaignGameStarter)
        {
            GameManager.Instance.AddTrigger(new CivilWarTrigger());
        }
    }
}