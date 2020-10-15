using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSI.Core.Objects;
using TaleWorlds.Core;
using TaleWorlds.CampaignSystem;
using BSI.Core.Manager;
using System.Collections.ObjectModel;
using TaleWorlds.CampaignSystem.Actions;
using System.Runtime.Remoting.Messaging;
using BSI.Core.Flags;

namespace BSI.Core.Managers
{
    public static class GameManager
    {
        internal static Game CurrentGame = Game.Current;

        internal static readonly Dictionary<IFaction, PlotManager> FactionManager = new Dictionary<IFaction, PlotManager>();

        internal static readonly PlotManager GlobalPlots = new PlotManager();

        private static readonly List<TriggerBase> TriggerManager = new List<TriggerBase>();

        internal static ReadOnlyCollection<TriggerBase> Triggers { get; set; }
        
        public static void LoadTrigger(TriggerBase trigger)
        {
            if (!TriggerManager.Contains(trigger)) { TriggerManager.Add(trigger); }
            Triggers = new ReadOnlyCollection<TriggerBase>(TriggerManager);
        }
          
        public static void NewGame()
        {
            foreach (Kingdom kingdom in Kingdom.All)
            {
                FactionManager.Add(kingdom, new PlotManager());
                foreach (Clan clan in kingdom.Clans)
                {
                    FactionManager.Add(clan, new PlotManager());
                }
            }
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
                
                foreach (IFaction faction in GameManager.FactionManager.Keys)
                {
                    Debug.AddEntry(faction.Name.ToString() + " || " + GameManager.FactionManager[faction].CurrentPlots.Count.ToString());
                }
                
                Debug.AddEntry("New Game Complete");

                
                        //REWORK ALL THIS SHIT
                //Step 1: Find Plot Types
                //Step 2: Foreach Plot Type
                //Step 3: switch case for UniqueTo
                //Step 4: loop through relevant faction
                //Step 5: Check Trigger OR Join && Apply result


                //if (!BSIManager.GameManager[kingdom].CurrentPlots.IsEmpty())
                //{
                //    foreach (Hero lord in kingdom.Lords)
                //    {
                //        bool temp = trigger.DoPlot(lord);
                //        Debug.AddEntry(lord.Clan.Kingdom.ToString() + " || " + lord.Clan.ToString() + " || " + lord.Name.ToString() + " || " + temp.ToString());
                //    }

                //}
                //else
                //{
                //    foreach (Plot factionPlot in BSIManager.GameManager[kingdom].CurrentPlots)
                //    {
                //        if (factionPlot.Trigger.Equals(trigger)) { factionPlot.CurrentGoal.Behavior.OnDailyTick(); }
                //        Debug.AddEntry(factionPlot.Name.ToString() + "\n\n" + factionPlot.Members.ToString());
                //    }
                //}



            }
        }
        //internal static class PlotCollector
        //{
        //    static PlotCollector() { }

        //    public static ReadOnlyCollection<Plot> PlotTypes = new ReadOnlyCollection<Plot>((IList<Plot>)Assembly.GetAssembly(typeof(Plot)).GetTypes().Where(t => t.IsSubclassOf(typeof(Plot))));
        //}
    } 
}
