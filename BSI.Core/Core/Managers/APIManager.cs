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
    public static class APIManager
    {
        internal static Dictionary<Type, Plot> PlotTypes = new Dictionary<Type, Plot>();

        internal static List<Trigger> TriggerList = new List<Trigger>();

        public static void LoadPlot(Plot plot)
        {
            PlotTypes.Add(plot.PlotType, plot);
            TriggerList.Add(plot.Trigger);
        }

        public class APIConnector : CampaignBehaviorBase
        {
            public override void RegisterEvents()
            {
                CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTickEvent));
            }

            public override void SyncData(IDataStore dataStore)
            {

            }

            internal void DailyTickEvent()
            {
                GameManager.UpdateKingdoms();

                foreach (Plot plot in PlotTypes.Values)
                {
                    foreach (FactionInfo kingdom in GameManager.Kingdoms)
                    {
                        foreach (Hero lord in kingdom.Lords)
                        {
                            if (kingdom.PlotManager.FactionPlots.IsEmpty())
                            {
                                plot.Trigger.DoPlot(lord);
                            }
                            else
                            {
                                foreach (Plot factionPlot in kingdom.PlotManager.FactionPlots)
                                {
                                    if (factionPlot.PlotType.Equals(plot.PlotType)) { factionPlot.CurrentBehavior.OnDailyTick(); }
                                }
                            }
                                
                        }
                    }
                }
            }
        }
    }
}
