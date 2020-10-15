using BSI.Core.Manager;
using BSI.Core.Objects;
using BSI.Core.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;

namespace BSI.Core.Managers
{
    public static class GameManager
    {
        internal static Game CurrentGame = Game.Current;

        internal static readonly Dictionary<IFaction, PlotManager> FactionManager = new Dictionary<IFaction, PlotManager>();

        internal static readonly PlotManager GlobalPlots = new PlotManager();

        private static readonly List<TriggerBase> Triggers = new List<TriggerBase>();

        internal static ReadOnlyCollection<TriggerBase> TriggerManager { get; set; }
        
        public static void LoadTrigger(TriggerBase trigger)
        {
            if (!Triggers.Contains(trigger)) { Triggers.Add(trigger); }
            TriggerManager = new ReadOnlyCollection<TriggerBase>(Triggers);
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

                DailyRecruitmentCheck();

                foreach (IFaction faction in GameManager.FactionManager.Keys)
                {
                    Debug.AddEntry(faction.Name.ToString() + " || " + GameManager.FactionManager[faction].CurrentPlots.Count.ToString());
                }


                //REWORK ALL THIS SHIT
                //Step 1: Find Plot Types
                //Step 2: Foreach Plot Type
                //Step 3: switch case for UniqueTo
                //Step 4: loop through relevant faction
                //Step 5: Check Trigger OR Join && Apply result






            }

            internal void DailyRecruitmentCheck()
            {
                foreach (TriggerBase trigger in TriggerManager)
                {
                    Debug.AddEntry("Entered Trigger Loop");

                    List<IFaction> HavePlot = new List<IFaction>();
                    bool HasGlobalPlot = false;
                    //Find Faction/Scopes with an already existing Plot of this type
                    switch (trigger.Uniqueto)
                    {
                        case Uniqueto.Global:
                            foreach (Plot plot in GameManager.GlobalPlots.CurrentPlots.Where(plot => plot.Trigger.GetType() == trigger.GetType())) { HasGlobalPlot = true; break; }
                            break;
                        case Uniqueto.Kingdom:
                            foreach (Kingdom faction in GameManager.FactionManager.Keys)
                            {
                                foreach (Plot plot in GameManager.FactionManager[faction].CurrentPlots.Where(plot => plot.Trigger.GetType() == trigger.GetType())) { HavePlot.Add(faction); break; }
                            }
                            break;
                        case Uniqueto.Clan:
                            foreach (Clan faction in GameManager.FactionManager.Keys)
                            {
                                foreach (Plot plot in GameManager.FactionManager[faction].CurrentPlots.Where(plot => plot.Trigger.GetType() == trigger.GetType())) { HavePlot.Add(faction); break; }
                            }
                            break;
                    }
                    
                    // Loop through those that don't and start a plot if needed
                    switch (trigger.Uniqueto)
                    {
                        case Uniqueto.Global:
                            if (!HasGlobalPlot)
                            {
                                foreach (Hero hero in Hero.All.Where(hero => HeroTools.IsClanLeader(hero)))
                                {
                                    if (trigger.DoPlot(hero)) { HasGlobalPlot = true; break; }
                                }
                            }
                            break;
                        case Uniqueto.Kingdom:
                            foreach (Kingdom faction in GameManager.FactionManager.Keys.Where(faction => !HavePlot.Contains(faction)))
                            {
                                foreach (Hero hero in faction.Lords.Where(hero => HeroTools.IsClanLeader(hero)))
                                {
                                    if (trigger.DoPlot(hero)) { HavePlot.Add(faction); break; }
                                }
                            }
                            break;
                        case Uniqueto.Clan:
                            foreach (Clan faction in GameManager.FactionManager.Keys.Where(faction => !HavePlot.Contains(faction)))
                            {
                                foreach (Hero hero in faction.Lords.Where(hero => HeroTools.IsClanLeader(hero)))
                                {
                                    if (trigger.DoPlot(hero)) { HavePlot.Add(faction); break; }
                                }
                            }
                            break;
                    }

                    //Loop through those that do and do Recruitment Checks & appropriate new Leader checks
                    foreach (IFaction faction in HavePlot)
                    {
                        foreach (Plot plot in GameManager.FactionManager[faction].CurrentPlots.Where(plot => plot.Trigger.GetType().Equals(trigger)))
                        {
                            foreach(Hero member in faction.Lords.Where(member => !plot.Members.Contains(member)))
                            {
                                if (plot.CurrentGoal.Behavior.DoPlot(member, plot))
                                {
                                    plot.CurrentGoal.Behavior.IsNewLeader(member, plot);
                                }
                                
                            }
                        }
                    }

                }
            }
        }
    } 
}
