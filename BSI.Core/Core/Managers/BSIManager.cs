using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using BSI.Core.Objects;
using TaleWorlds.Core;
using TaleWorlds.CampaignSystem;
using BSI.Core.Manager;

namespace BSI.Core.Managers
{
    public static class BSIManager
    {
        internal static Game CurrentGame;

        internal static readonly Dictionary<IFaction, PlotManager> GameManager = new Dictionary<IFaction, PlotManager>();

        internal static readonly PlotManager GlobalPlots = new PlotManager();



        

        public static void NewaGme(Game game)
        {
            BSIManager.CurrentGame = game;
        }

        public class BSIConnector : CampaignBehaviorBase
        {
            public override void RegisterEvents()
            {
                CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.OnDailyTick));
            }

            public override void SyncData(IDataStore dataStore)
            {

            }

            internal void OnDailyTick()
            {
                Debug.AddEntry("Tick registered");
                foreach (TriggerBase trigger in TriggerDict.Values)
                {
                    foreach (Kingdom kingdom in BSIManager.GameManager.Keys)
                    {
                        if (!BSIManager.GameManager[kingdom].CurrentPlots.IsEmpty())
                        {
                            foreach (Hero lord in kingdom.Lords)
                            {
                                bool temp = trigger.DoPlot(lord);
                                Debug.AddEntry(lord.Clan.Kingdom.ToString() + " || " + lord.Clan.ToString() + " || " + lord.Name.ToString() + " || " + temp.ToString());
                            }

                        }
                        else
                        {
                            foreach (Plot factionPlot in BSIManager.GameManager[kingdom].CurrentPlots)
                            {
                                if (factionPlot.Trigger.Equals(trigger)) { factionPlot.CurrentGoal.Behavior.OnDailyTick(); }
                                Debug.AddEntry(factionPlot.Name.ToString() + "\n\n" + factionPlot.Members.ToString());
                            }
                        }

                    }
                }
            }
        }
    }
}
