using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using BSI.Core.CoreObjects;
using TaleWorlds.Core;
using TaleWorlds.CampaignSystem;
using BSI.Manager;

namespace BSI.Core.Managers
{
    public static class BSIManager
    {
        internal static Dictionary<string, Trigger> TriggerDict = new Dictionary<string, Trigger>();

        public static void LoadTrigger(string name, Trigger trigger)
        {
            TriggerDict.Add(name, trigger);
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
                Debug.PrintMessage("Tick Now");
                GameManager.UpdateKingdoms();
                Debug.AddEntry("Kingdoms Updated");
                foreach (Trigger trigger in TriggerDict.Values)
                {
                    foreach (FactionInfo kingdom in GameManager.Kingdoms)
                    {                        
                        if (kingdom.PlotManager.FactionPlots.IsEmpty())
                        {
                            foreach (Hero lord in kingdom.Lords)
                            {
                                bool temp = trigger.DoPlot(lord);
                                Debug.AddEntry(lord.Clan.Kingdom.ToString() + " || " + lord.Clan.ToString() + " || " + lord.Name.ToString() + " || " + temp.ToString());
                            }
                             
                        }
                        else
                        {
                            foreach (Plot factionPlot in kingdom.PlotManager.FactionPlots)
                            {
                                if (factionPlot.Trigger.Equals(trigger)) { factionPlot.CurrentBehavior.OnDailyTick(); }
                                Debug.AddEntry(factionPlot.Name.ToString() + "\n\n" + factionPlot.Members.ToString());
                            }
                        }
                                
                        
                    }
                }
            }
        }
    }
}
