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
using TaleWorlds.SaveSystem.Definition;

namespace BSI.Core.Managers
{
    [Serializable]
    public static class GameManager
    {
        //internal static readonly Game CurrentGame;

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
            }
            foreach (Clan clan in Clan.All)
            {
                FactionManager.Add(clan, new PlotManager());
            }
        }

        public class EventManager : CampaignBehaviorBase
        {
            public override void RegisterEvents()
            {
                CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.OnDailyTick));
                CampaignEvents.KingdomCreatedEvent.AddNonSerializedListener(this, (kingdom) => AddFaction(kingdom));
                CampaignEvents.ClanChangedKingdom.AddNonSerializedListener(this, (clan, oldkingdom, newkingdom, rebel, other) => ClanChangeKingdom(clan, oldkingdom, newkingdom, rebel, other));
                CampaignEvents.KingdomDestroyedEvent.AddNonSerializedListener(this, (kingdom) => RemoveFaction(kingdom));
                CampaignEvents.OnClanDestroyedEvent.AddNonSerializedListener(this, (clan) => RemoveFaction(clan));
                CampaignEvents.OnBeforeSaveEvent.AddNonSerializedListener(this, new Action(this.Saving));
            }

            private void ClanChangeKingdom(Clan clan, Kingdom oldkingdom, Kingdom newKingdom, bool byRebellion = true, bool other = false)
            {
                foreach (Plot plot in GameManager.FactionManager[oldkingdom].CurrentPlots)
                {
                    if (plot.Members.Contains(clan.Leader) && !GameManager.FactionManager[newKingdom].IsPlotFaction)
                    {
                        plot.RemoveMember(clan.Leader);
                    }
                }
            }

            private void Saving()
            {
                
            }

            public static void AddFaction(IFaction faction, bool rebel = false)
            {
                if (!GameManager.FactionManager.ContainsKey(faction))
                {
                    GameManager.FactionManager.Add(faction, new PlotManager());
                }
                if (faction.GetType() == typeof(Kingdom)) { KingdomTools.UpdateKingdomList(); }

                GameManager.FactionManager[faction].IsPlotFaction = rebel;
            }

            public static void RemoveFaction(IFaction faction)
            {
                GameManager.FactionManager.Remove(faction);
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

                foreach (IFaction faction in GameManager.FactionManager.Keys.Where(faction => GameManager.FactionManager[faction].CurrentPlots.Count() > 0))
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
                            foreach (IFaction faction in GameManager.FactionManager.Keys.Where(faction => faction.IsKingdomFaction))
                            {
                                foreach (Plot plot in GameManager.FactionManager[faction].CurrentPlots.Where(plot => plot.Trigger.GetType() == trigger.GetType())) { HavePlot.Add(faction); break; }
                            }
                            break;
                        case Uniqueto.Clan:
                            foreach (IFaction faction in GameManager.FactionManager.Keys.Where(faction => faction.IsClan))
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
                            foreach (IFaction faction in GameManager.FactionManager.Keys.Where(faction => faction.IsKingdomFaction && !HavePlot.Contains(faction)))
                            {
                                foreach (Hero hero in faction.Lords.Where(hero => HeroTools.IsClanLeader(hero)))
                                {
                                    if (trigger.DoPlot(hero)) { HavePlot.Add(faction); break; }
                                }
                            }
                            break;
                        case Uniqueto.Clan:
                            foreach (IFaction faction in GameManager.FactionManager.Keys.Where(faction => faction.IsClan && !HavePlot.Contains(faction)))
                            {
                                foreach (Hero hero in faction.Lords.Where(hero => HeroTools.IsClanLeader(hero)))
                                {
                                    if (trigger.DoPlot(hero)) { HavePlot.Add(faction); break; }
                                }
                            }
                            break;
                    }
                    
                    Debug.AddEntry("Plots exist in " + HavePlot.Count.ToString() + " factions");

                    //Loop through those that do and do Recruitment Checks & appropriate new Leader checks
                    foreach (IFaction faction in HavePlot)
                    {
                        foreach (Plot plot in GameManager.FactionManager[faction].CurrentPlots.Where(plot => plot.Trigger.GetType().Equals(trigger.GetType())))
                        {
                            plot.CurrentGoal.Behavior.OnDailyTick(plot);
                        }
                    }

                }
            }
        }

        
    } 
}
